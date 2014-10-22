using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Foundation
{
    public class SiteCacheKey
    {
        /// <summary>
        /// 分类文章数
        /// </summary>
        public static string CateArticleCount = "CateArticleCount_";


        /// <summary>
        /// 文章排行缓存
        /// </summary>
        public static string ArticleTop10Article = "ArticleTop10Article";


        /// <summary>
        /// 单个栏目
        /// </summary>
        public static string CategorySingle = "CategorySingle";

        /// <summary>
        /// 栏目导航树
        /// </summary>
        public static string MenuTree = "Content_CategoryTree";

        /// <summary>
        /// 配置文件缓存-网站模板配置
        /// </summary>
        public static string ConfigFrontTemplate = "Config_FrontTemplate";


        /// <summary>
        /// 展示层的Dll缓存
        /// </summary>
        public static string AssemblyPresentation = "Assembly_Presentation";

    }
}
