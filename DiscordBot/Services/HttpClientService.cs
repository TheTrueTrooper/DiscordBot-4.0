using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DiscordBot.Services
{
    public class HttpClientService
    {
        public string BaseURL { get; private set; }

        public HttpClientService(string BaseURL = null)
        {
            this.BaseURL = BaseURL;
        }

        public string GetResponseAsText(string URLEnding, object Vars = null)
        {
            return GetResponseAsText(GetResponseStream(URLEnding, Vars));
        }

        public string GetResponseAsText(Stream Stream)
        {
            StreamReader Reader = new StreamReader(Stream);
            return Reader.ReadToEnd();
        }

        public Stream GetResponseStream(string URLEnding, object Vars = null)
        {
            return GetResponse(URLEnding, Vars).GetResponseStream();
        }

        public HttpWebResponse GetResponse(string URLEnding, object Vars = null)
        {
            string URL = BaseURL != null ? $"{BaseURL}/{URLEnding}" : URLEnding;
            if (Vars != null)
                URL = URL + GetVarsStringFromObj(Vars);

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URL);
            return (HttpWebResponse)Request.GetResponse();
        }

        public List<string> GetNodeFromDoc(string Source, string HtmlTag, object Attributes = null)
        {
            List<string> Return = new List<string>();
            Dictionary<string, string> AttributesLookup = new Dictionary<string, string>();
            if (Attributes != null)
            {
                Type ObjType = Attributes.GetType();
                List<PropertyInfo> Props = new List<PropertyInfo>(ObjType.GetProperties());
                foreach (PropertyInfo Prop in Props)
                {
                    if (Prop.PropertyType.IsPrimitive || Prop.PropertyType == typeof(string))
                    {
                        object propValue = Prop.GetValue(Attributes, null);
                        AttributesLookup.Add(Prop.Name.ToLower(), propValue.ToString());
                    }
                    // Do something with propValue
                }
            }
            HtmlParser Parser = new HtmlParser();
            IHtmlDocument Document = Parser.ParseDocument(Source);
            foreach (IElement e in Document.All.Where(x => x.LocalName == HtmlTag && AttributesLookup.All(y => x.GetAttribute(y.Key) == y.Value)).ToList())
                Return.Add(e.OuterHtml);
            return Return;
        }

        public List<string> GetNodeFromDocByClass(string Source, string HtmlTag, params string[] Classes)
        {
            List<string> Return = new List<string>();
            HtmlParser Parser = new HtmlParser();
            IHtmlDocument Document = Parser.ParseDocument(Source);
            foreach (IElement e in Document.All.Where(x => x.LocalName == HtmlTag && Classes.All(y => x.ClassList.Contains(y))).ToList())
                Return.Add(e.OuterHtml);
            return Return;
        }

        public List<string> GetNodeChildren(string Source)
        {
            List<string> Return = new List<string>();
            HtmlParser Parser = new HtmlParser();
            IHtmlDocument Document = Parser.ParseDocument(Source);
            foreach (IElement e in Document.Body.Children[0].Children)
                Return.Add(e.OuterHtml);
            return Return;
        }

        public string GetNodesInnerHtml(string Source)
        {
            HtmlParser Parser = new HtmlParser();
            IHtmlDocument Document = Parser.ParseDocument(Source);
            return Document.Body.Children[0].InnerHtml;
        }

        public string GetNodesAttribute(string Source, string Attribute)
        {
            HtmlParser Parser = new HtmlParser();
            IHtmlDocument Document = Parser.ParseDocument(Source);
            return Document.Body.Children[0].GetAttribute(Attribute);
        }

        string GetVarsStringFromObj(object Obj)
        {
            Type ObjType = Obj.GetType();
            List<PropertyInfo> Props = new List<PropertyInfo>(ObjType.GetProperties());
            string GetVars = "?";
            foreach (PropertyInfo Prop in Props)
            {
                if (Prop.PropertyType.IsPrimitive || Prop.PropertyType == typeof(string))
                {
                    object propValue = Prop.GetValue(Obj, null);
                    GetVars += Prop.Name + "=" + propValue + "&";
                }
                // Do something with propValue
            }
            return GetVars.Remove(GetVars.Length - 1);
        }
    }
}
