﻿/*****************************************************
 * 本类库的核心系 JNTemplate
 * 作者：翅膀的初衷 QQ:4585839
 * Mail: i@Jiniannet.com
 * 网址：http://www.JiNianNet.com
 *****************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using HC.TemplateEngine.Parser.Node;

namespace HC.TemplateEngine.Parser
{
    /// <summary>
    /// 分析器
    /// </summary>
    public class Parser
    {
        private readonly static List<ITagParser> collection;

        static Parser()
        {
            collection = new List<ITagParser>();
            Reset();
        }
        /// <summary>
        /// 分析标签
        /// </summary>
        /// <param name="parser">TemplateParser</param>
        /// <param name="tc">Token集合</param>
        /// <returns></returns>
        public static Tag Parse(TemplateParser parser, TokenCollection tc)
        {
            Tag t;
            for (Int32 i = 0; i < collection.Count; i++)
            {
                t = collection[i].Parse(parser, tc);
                if (t != null)
                {
                    t.FirstToken = tc.First;

                    if (t.Children.Count == 0 || (t.LastToken = t.Children[t.Children.Count - 1].LastToken ?? t.Children[t.Children.Count - 1].FirstToken) == null || tc.Last.CompareTo(t.LastToken) > 0)
                    {
                        t.LastToken = tc.Last;
                    }
                    return t;
                }
            }
            return null;
        }
        /// <summary>
        /// 添加一个标签分析器
        /// </summary>
        /// <param name="item">标签分析器</param>
        public static void Add(ITagParser item)
        {
            collection.Add(item);
        }
        /// <summary>
        /// 插入一个标签分析器
        /// </summary>
        /// <param name="index">插入索引</param>
        /// <param name="item">标签分析器</param>
        public static void Insert(Int32 index, ITagParser item)
        {
            collection.Insert(index, item);
        }
        /// <summary>
        /// 清除所有分析器
        /// </summary>
        public static void Clear()
        {
            collection.Clear();
        }
        /// <summary>
        /// 重设/初始默认分析器列表
        /// </summary>
        public static void Reset()
        {
            collection.Clear();
            collection.Add(new BooleanParser());
            collection.Add(new NumberParser());
            collection.Add(new EleseParser());
            collection.Add(new EndParser());
            collection.Add(new VariableParser());
            collection.Add(new StringParser());
            collection.Add(new ForeachParser());
            collection.Add(new ForParser());
            collection.Add(new SetParser());
            collection.Add(new IfParser());
            collection.Add(new ElseifParser());
            collection.Add(new LoadParser());
            collection.Add(new IncludeParser());
            //collection.Add(new ExpressionParser());
            //collection.Add(new ReferenceParser());
            collection.Add(new FunctionParser());
            collection.Add(new ComplexParser());
            
        }
    }
}
