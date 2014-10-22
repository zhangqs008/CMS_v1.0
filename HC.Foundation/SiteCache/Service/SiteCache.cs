//--------------------------------------------------------------------------------
// 文件描述：缓存类
// 文件作者：张清山
// 创建日期：2013-12-10 16:49:33
// 修改记录：
//--------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace HC.Foundation
{
    /// <summary>
    /// 全站缓存类
    /// </summary>
    public static class SiteCache
    {
        /// <summary>
        /// 一天
        /// </summary>
        public const int DayFactor = 17280;

        /// <summary>
        /// 一小时
        /// </summary>
        public const int HourFactor = 720;

        /// <summary>
        /// 一分钟
        /// </summary>
        public const int MinuteFactor = 12;

        /// <summary>
        /// 一秒
        /// </summary>
        public const double SecondFactor = 0.2;

        private static readonly Cache siteCache = InitSiteCache();

        private static int _factor = 5;

        /// <summary>
        /// 时间因子
        /// </summary>
        public static int Factor
        {
            get { return _factor; }
            set { _factor = value; }
        }

        /// <summary>
        /// 获取缓存列表
        /// </summary>
        /// <returns>返回缓存列表</returns>
        public static IList<CacheInfo> CurrentCacheList
        {
            get
            {
                IDictionaryEnumerator cacheEnum = siteCache.GetEnumerator();
                IList<CacheInfo> cacheInfoList = new List<CacheInfo>();

                while (cacheEnum.MoveNext())
                {
                    var cacheInfo = new CacheInfo();
                    cacheInfo.Key = cacheEnum.Key.ToString();
                    string cacheValue = cacheEnum.Value.ToString();
                    if (cacheValue.Length > 100)
                    {
                        cacheValue = cacheValue.Substring(0, 100);
                    }

                    cacheInfo.Value = cacheValue;
                    cacheInfoList.Add(cacheInfo);
                }

                return cacheInfoList;
            }
        }

        /// <summary>
        /// 从应用程序缓存移除全部项
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator cacheEnum = siteCache.GetEnumerator();
            var al = new ArrayList();
            while (cacheEnum.MoveNext())
            {
                al.Add(cacheEnum.Key);
            }

            foreach (string key in al)
            {
                siteCache.Remove(key);
            }
        }

        /// <summary>
        /// 通过正则移除对应的缓存
        /// </summary>
        /// <param name="pattern">正则</param>
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator cacheEnum = siteCache.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            var al = new ArrayList();
            while (cacheEnum.MoveNext())
            {
                if (regex.IsMatch(cacheEnum.Key.ToString()))
                {
                    al.Add(cacheEnum.Key);
                }
            }

            foreach (string key in al)
            {
                siteCache.Remove(key);
            }
        }

        /// <summary>
        /// 从应用程序缓存移除指定项
        /// </summary>
        /// <param name="key">要移除缓存项的string标识符</param>
        public static void Remove(string key)
        {
            siteCache.Remove(key);
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        public static void Insert(string key, object value)
        {
            Insert(key, value, null, 60 * 60);
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dep">缓存依赖</param>
        public static void Insert(string key, object value, CacheDependency dep)
        {
            Insert(key, value, dep, HourFactor * 12);
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="seconds">缓存过期时间</param>
        public static void Insert(string key, object value, int seconds)
        {
            Insert(key, value, null, seconds);
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="seconds">缓存过期时间</param>
        /// <param name="priority">缓存级别</param>
        public static void Insert(string key, object value, int seconds, CacheItemPriority priority)
        {
            Insert(key, value, null, seconds, priority);
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dep">缓存依赖</param>
        /// <param name="seconds">缓存过期时间</param>
        public static void Insert(string key, object value, CacheDependency dep, int seconds)
        {
            Insert(key, value, dep, seconds, CacheItemPriority.NotRemovable);
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dep">缓存依赖</param>
        /// <param name="seconds">缓存过期时间</param>
        /// <param name="priority">缓存级别</param>
        public static void Insert(string key, object value, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (value != null)
            {
                siteCache.Insert(key, value, dep, DateTime.Now.AddSeconds(Factor * seconds), TimeSpan.Zero, priority, null);
            }
        }

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="secondFactor">时间因子</param>
        public static void MicroInsert(string key, object value, int secondFactor)
        {
            if (value != null)
            {
                siteCache.Insert(key, value, null, DateTime.Now.AddSeconds(Factor * secondFactor), TimeSpan.Zero);
            }
        }

        /// <summary>
        /// 插入缓存项，最大时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        public static void Max(string key, object value)
        {
            Max(key, value, null);
        }

        /// <summary>
        /// 插入缓存项，最大时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dep">缓存依赖</param>
        public static void Max(string key, object value, CacheDependency dep)
        {
            if (value != null)
            {
                siteCache.Insert(key, value, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public static object Get(string key)
        {
            return siteCache[key];
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public static Cache InitSiteCache()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                return context.Cache;
            }
            return HttpRuntime.Cache;
        }
    }
}