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
        public const string InfoTag = "DJ";

        public const string EmojiTag = "🎶";

        public static string Help = $"{EmojiTag}_**For {InfoTag} Commands:**_{EmojiTag}\n```{InfoTag} commands are as follows:{JoinVoiceHelp}{PlayFileHelp}{LeaveVoiceHelp}```";

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

        const string JoinVoiceHelp = "\n-JoinVoice command causes the bot to join your current voice channel. command can be called with ~joinvoice or the short hand alias ~jv";
        [Command("joinvoice", RunMode = RunMode.Async)]
        [Alias("jv")]
        public async Task JoinChannel(IVoiceChannel Channel = null)
        {
            // Get the audio channel
            await AudioService.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        const string PlayFileHelp = "\n-PlayFile command causes the bot to join your current voice channel and play a audio file (bot must have the audio file with it). command can be called with ~playfile or the short hand alias ~pl";
        [Command("playfile", RunMode = RunMode.Async)]
        [Alias("pl")]
        public async Task PlayFile([Remainder] string Song)
        {
            // Get the audio channel
            await AudioService.SendAudioFFMPEGFileAsync(Context.Guild, Context.Channel, Song);
        }

        const string LeaveVoiceHelp = "\n-LeaveVoice command causes the bot to leave your current voice channel. command can be called with ~leavevoice or the short hand alias ~lv";
        [Command("leavevoice", RunMode = RunMode.Async)]
        [Alias("lv")]
        public async Task LeaveChannel()
        {
            // Get the audio channel
            await AudioService.LeaveAudio(Context.Guild);
        }
    }
}
