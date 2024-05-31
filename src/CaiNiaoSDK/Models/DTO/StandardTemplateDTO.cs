using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace CaiNiaoSDK.Models.DTO
{
    public class StandardTemplateDTO
    {
        [JsonProperty("standardTemplateUrl")] public string StandardTemplateUrl { get; set; }
        [JsonProperty("standardTemplateName")] public string StandardTemplateName { get; set; }
        [JsonProperty("standardWaybillType")] public int StandardWaybillType { get; set; }

        public string TypeStringName
        {
            get
            {
                switch (StandardWaybillType)
                {
                    case 1: return "快递标准面单";
                    case 2: return "快递三联面单";
                    case 3: return "快递便携式三联单";
                    case 4: return "快运标准面单";
                    case 5: return "快运三联面单,";
                    case 6: return "快递一联单";
                    default: return "未知";
                }
            }
        }

        public string Size
        {
            get
            {
                try
                {
                    var webClient = new WebClient();
                    var response = webClient.DownloadString(this.StandardTemplateUrl);
                    response = Regex.Replace(response, @"<%((?!%>)[\S\s])*%>", "");

                    var rootElement = XElement.Parse(response);
                    return rootElement.Attribute("width")?.Value + "*" + rootElement.Attribute("height")?.Value;
                }
                catch (Exception e)
                {
                    return "获取失败";
                }
            }
        }
    }
}