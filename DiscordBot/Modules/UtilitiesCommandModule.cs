using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.Commands;
using DiscordBot.Services;

namespace DiscordBot.Modules
{
    public class UtilitiesCommandModule : ModuleBase<SocketCommandContext>
    {
        public const string InfoTag = "Utilities";
    
        public const string EmojiTag = "🛠️";

        public static string Help = $"{EmojiTag}_**For {InfoTag} Commands:**_{EmojiTag}\n```{InfoTag} commands are as follows:{YoutubeSearchHelp}```\nYou may need admin privilege to use some commands to prevent greaving";

        private readonly YoutubeService YoutubeService;

        public UtilitiesCommandModule(YoutubeService YService)
        {
            YoutubeService = YService;
        }

        const string YoutubeSearchHelp = "\n-YoutubeSearchHelp command causes the bot to look up youtube for you. command can be called with ~youtubesearch or the short hand alias ~yts";
        [Command("youtubesearch", RunMode = RunMode.Async)]
        [Alias("yts")]
        public async Task YouTubeSearch([Remainder] string Search)
        {
            string Message = "";
            // Get the audio channel
            List<YoutubeSearchData> Data = YoutubeService.GetSearchList(Search);
            foreach (YoutubeSearchData Result in Data)
                Message += $"{Result.Title} - {Result.VideoPageLink}\n";
            await Context.Channel.SendMessageAsync(Message);
        }

    }
}
