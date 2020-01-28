using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DiscordBot.Services;

namespace DiscordBot.Modules
{
    public class DJCommandModule : ModuleBase<SocketCommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private readonly AudioService AudioService;

        private readonly YoutubeService YoutubeService;

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public DJCommandModule(AudioService AService, YoutubeService YService)
        {
            AudioService = AService;
            YoutubeService = YService;
        }

        [Command("joinvoice", RunMode = RunMode.Async)]
        [Alias("jv")]
        public async Task JoinChannel(IVoiceChannel Channel = null)
        {
            // Get the audio channel
            await AudioService.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("playfile", RunMode = RunMode.Async)]
        [Alias("pl")]
        public async Task PlayFile([Remainder] string Song)
        {
            // Get the audio channel
            await AudioService.SendAudioFFMPEGFileAsync(Context.Guild, Context.Channel, Song);
        }

        [Command("leavevoice", RunMode = RunMode.Async)]
        [Alias("lv")]
        public async Task LeaveChannel()
        {
            // Get the audio channel
            await AudioService.LeaveAudio(Context.Guild);
        }

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
