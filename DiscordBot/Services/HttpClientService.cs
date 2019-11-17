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

        public List<XElement> GetNodeFromID(string Source, string IDKey)
        {
            HtmlDocument doc = new HtmlDocument();
        }

        string GetVarsStringFromObj(object Obj)
        {
            Type ObjType = Obj.GetType();
            List<PropertyInfo> props = new List<PropertyInfo>(ObjType.GetProperties());
            string GetVars = "?";
            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                {
                    object propValue = prop.GetValue(Obj, null);
                    GetVars += prop.Name + "=" + propValue + "&";
                }
                // Do something with propValue
            }
            return GetVars.Remove(GetVars.Length - 1);
        }
    }
}
