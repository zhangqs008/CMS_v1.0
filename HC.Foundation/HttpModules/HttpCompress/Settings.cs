//--------------------------------------------------------------------------------
// 文件描述：gzip模块
// 文件作者：全体开发人员
// 创建日期：2014-2-12
//--------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace HC.Foundation.HttpModules.HttpCompress
{
    /// <summary>
    ///     This class encapsulates the settings for an HttpCompressionModule
    /// </summary>
    public sealed class Settings
    {
        private readonly StringCollection _excludedPaths;
        private readonly StringCollection _excludedTypes;
        private CompressionLevels _compressionLevel;
        private Algorithms _preferredAlgorithm;

        /// <summary>
        ///     Create an HttpCompressionModuleSettings from an XmlNode
        /// </summary>
        /// <param name="node">The XmlNode to configure from</param>
        public Settings(XmlNode node)
            : this()
        {
            AddSettings(node);
        }

        private Settings()
        {
            _preferredAlgorithm = Algorithms.Default;
            _compressionLevel = CompressionLevels.Default;
            _excludedTypes = new StringCollection();
            _excludedPaths = new StringCollection();
        }

        /// <summary>
        ///     The default settings.  Deflate + normal.
        /// </summary>
        public static Settings Default
        {
            get { return new Settings(); }
        }

        /// <summary>
        ///     The preferred algorithm to use for compression
        /// </summary>
        public Algorithms PreferredAlgorithm
        {
            get { return _preferredAlgorithm; }
        }

        /// <summary>
        ///     The preferred compression level
        /// </summary>
        public CompressionLevels CompressionLevel
        {
            get { return _compressionLevel; }
        }

        /// <summary>
        ///     Get the current settings from the xml config file
        /// </summary>
        /// <returns>参数设置</returns>
        public static Settings GetSettings()
        {
            var settings = new Settings();
            settings._preferredAlgorithm = Algorithms.GZip;
            settings._compressionLevel = CompressionLevels.High;
            settings._excludedTypes.Add("image/jpeg");
            settings._excludedTypes.Add("image/gif");
            return settings;
        }

        /// <summary>
        ///     Suck in some more changes from an XmlNode.  Handy for config file parenting.
        /// </summary>
        /// <param name="node">The node to read from</param>
        public void AddSettings(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            XmlAttribute xmlAttrAlgorithm = node.Attributes["preferredAlgorithm"];

            if (xmlAttrAlgorithm != null)
            {
                try
                {
                    _preferredAlgorithm = (Algorithms)Enum.Parse(typeof(Algorithms), xmlAttrAlgorithm.Value, true);
                }
                catch (ArgumentException)
                {
                }
            }

            XmlAttribute compressionLevels = node.Attributes["compressionLevel"];

            if (compressionLevels != null)
            {
                try
                {
                    _compressionLevel =
                        (CompressionLevels)Enum.Parse(typeof(CompressionLevels), compressionLevels.Value, true);
                }
                catch (ArgumentException)
                {
                }
            }

            ParseExcludedTypes(node.SelectSingleNode("excludedMimeTypes"));
            ParseExcludedPaths(node.SelectSingleNode("excludedPaths"));
        }

        /// <summary>
        ///     Checks a given mime type to determine if it has been excluded from compression
        /// </summary>
        /// <param name="mimetype">The MimeType to check.  Can include wildcards like image/* or */xml.</param>
        /// <returns>true if the mime type passed in is excluded from compression, false otherwise</returns>
        public bool IsExcludedMimeType(string mimetype)
        {
            if (mimetype == null)
            {
                return true;
            }

            return _excludedTypes.Contains(mimetype.ToLower());
        }

        /// <summary>
        ///     Looks for a given path in the list of paths excluded from compression
        /// </summary>
        /// <param name="relUrl">the relative url to check</param>
        /// <returns>true if excluded, false if not</returns>
        public bool IsExcludedPath(string relUrl)
        {
            // 排除 .axd 和图片
            string fileExt = Path.GetExtension(relUrl);

            if (!string.IsNullOrEmpty(fileExt)
                && (fileExt.ToLower() == ".axd" ||
                fileExt.ToLower() == ".jpg" ||
                fileExt.ToLower() == ".png" ||
                fileExt.ToLower() == ".gif" ||
                fileExt.ToLower() == ".bmp"
                ))
            {
                return true;
            }

            return _excludedPaths.Contains(relUrl.ToLower());
        }

        private void ParseExcludedTypes(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                switch (node.ChildNodes[i].LocalName)
                {
                    case "add":
                        if (node.ChildNodes[i].Attributes["type"] != null)
                        {
                            _excludedTypes.Add(node.ChildNodes[i].Attributes["type"].Value.ToLower());
                        }

                        break;
                    case "delete":
                        if (node.ChildNodes[i].Attributes["type"] != null)
                        {
                            _excludedTypes.Remove(node.ChildNodes[i].Attributes["type"].Value.ToLower());
                        }

                        break;
                }
            }
        }

        private void ParseExcludedPaths(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                switch (node.ChildNodes[i].LocalName)
                {
                    case "add":
                        if (node.ChildNodes[i].Attributes["path"] != null)
                        {
                            _excludedPaths.Add(node.ChildNodes[i].Attributes["path"].Value.ToLower());
                        }

                        break;
                    case "delete":
                        if (node.ChildNodes[i].Attributes["path"] != null)
                        {
                            _excludedPaths.Remove(node.ChildNodes[i].Attributes["path"].Value.ToLower());
                        }

                        break;
                }
            }
        }
    }
}