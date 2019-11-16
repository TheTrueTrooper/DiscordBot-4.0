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

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public DJCommandModule(AudioService Service)
        {
            AudioService = Service;
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
    }
}
