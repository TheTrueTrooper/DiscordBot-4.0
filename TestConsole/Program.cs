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
            YoutubeService Client = new YoutubeService();
            List<YoutubeSearchData> Source = Client.GetSearchList("cat");
            Console.ReadKey();
        }
    }
}
