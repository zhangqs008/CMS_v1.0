﻿/*****************************************************
 * 本类库的核心系 JNTemplate
 * 作者：翅膀的初衷 QQ:4585839
 * Mail: i@Jiniannet.com
 * 网址：http://www.JiNianNet.com
 *****************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.TemplateEngine.Parser.Node
{
    public class ElseTag : ElseifTag
    {
        public override Boolean ToBoolean(TemplateContext context)
        {
            return true;
        }
    }
}
