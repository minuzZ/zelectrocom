using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace ZelectroCom.Web.Infrastructure.Helpers
{
    public class HtmlParserHelper : IHtmlParserHelper
    {
        /// <summary>
        /// Gets part of html before "page-break".
        /// </summary>
        /// <param name="articleHtml">Full html</param>
        /// <param name="indexHtml">Resulted part of html</param>
        /// <returns>false, if page-break was not found</returns>
        public bool GetIndexHtmlFromArticleText(string articleHtml, out string indexHtml)
        {
            bool result = false;
            HtmlDocument articleDoc = new HtmlDocument();
            HtmlDocument indexDoc = new HtmlDocument();
            articleDoc.LoadHtml(articleHtml);

            indexHtml = string.Empty;

            foreach (var node in articleDoc.DocumentNode.ChildNodes)
            {
                string styleValue = node.GetAttributeValue("style", String.Empty);
                if (styleValue == "page-break-after: always")
                {
                    result = true;
                    var stringBuilder = new StringBuilder();
                    using (TextWriter writer = new StringWriter(stringBuilder))
                    {
                        indexDoc.Save(writer);
                        indexHtml = writer.ToString();
                    }
                    break;
                }
                indexDoc.DocumentNode.AppendChild(node);
            }

            
            return result;
        }
    }
}