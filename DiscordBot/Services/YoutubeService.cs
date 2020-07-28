using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class YoutubeService
    {
        public const string VideoPageLinkBase = "/watch?v=";
        public const string VideoEmbedLinkBase = "/embed/";
        public const string BaseURL = "https://www.youtube.com";

        HttpClientService HttpClient;

        public YoutubeService()
        {
            HttpClient = new HttpClientService(BaseURL);
        }

        public List<YoutubeSearchData> GetSearchList(string Search)
        {
            string Source = HttpClient.GetResponseAsText("results", new { search_query = Search });
            Source = HttpClient.GetNodeFromDoc(Source, "div", new { id = "content" })[0];
            Source = HttpClient.GetNodeFromDoc(Source, "ul", new { @class = "shelf-content" })[0];
            List<string> Items = HttpClient.GetNodeChildren(Source);
            List<YoutubeSearchData> SearchData = new List<YoutubeSearchData>();

            foreach(string Item in Items)
            {
                YoutubeSearchData ItemData = new YoutubeSearchData();
                ItemData.Title = HttpClient.GetNodesInnerHtml(HttpClient.GetNodeFromDocByClass(Item, "a", "yt-uix-tile-link")[0]);
                ItemData.VideoID = HttpClient.GetNodesAttribute(HttpClient.GetNodeFromDocByClass(Item, "a", "yt-uix-tile-link")[0], "href").Replace("/" + VideoPageLinkBase, "");
                ItemData.Time = HttpClient.GetNodesInnerHtml(HttpClient.GetNodeFromDocByClass(Item, "span", "accessible-description")[0]).Trim().Remove(0, 1).Trim();
                ItemData.Descpt = HttpClient.GetNodesInnerHtml(HttpClient.GetNodeFromDocByClass(Item, "div", "yt-lockup-description")[0]).Trim();
                //string VidPageSource = HttpClient.GetResponseAsText(ItemData.VideoLink);
                //Console.WriteLine(VidPageSource);
                SearchData.Add(ItemData);
            }

            return SearchData;
        }
    }

    public class YoutubeSearchData
    {
        public string Title { get; internal set; }
        public string Time { get; internal set; }
        public string VideoID { get; internal set; }
        public string Descpt { get; internal set; }
        public string VideoPageLink { get => YoutubeService.BaseURL + YoutubeService.VideoPageLinkBase + VideoID; }
        public string VideoEmbedLink { get => YoutubeService.BaseURL + YoutubeService.VideoEmbedLinkBase + VideoID; }

    }
}
