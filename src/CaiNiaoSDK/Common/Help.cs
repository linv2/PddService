using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CaiNiaoSDK.Common
{
    public class Help
    {
        public static string GetTemplateSize(string content)
        {
            try
            {
                var response = Regex.Replace(content, @"<%((?!%>)[\S\s])*%>", "");
                var rootElement = XElement.Parse(response);
                return rootElement.Attribute("width")?.Value + "*" + rootElement.Attribute("height")?.Value;
            }
            catch (Exception e)
            {
                return "解析失败";
            }

        }

        public static string GetTemplateContent(string content)
        {
           return Regex.Replace(content, @"<%((?!%>)[\S\s])*%>", "");
            
        }
    }
}