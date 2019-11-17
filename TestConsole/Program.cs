using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DiscordBot.Services;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClientService Client = new HttpClientService("https://www.youtube.com");
            string Source = Client.GetResponseAsText("results", new { search_query = "url" });
            Console.WriteLine(Source);
            List<XElement> SubNodes = Client.GetNodeFromID(Source, "id=\"content\"");
            Console.WriteLine($"Master node:\n\t{SubNodes}");
            Console.ReadLine();
        }
    }
}
