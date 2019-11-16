using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild Guild, IVoiceChannel Target)
        {
            IAudioClient Client;
            if (ConnectedChannels.TryGetValue(Guild.Id, out Client))
            {
                return;
            }
            if (Target.Guild.Id != Guild.Id)
            {
                return;
            }

            IAudioClient AudioClient = await Target.ConnectAsync();

            if (ConnectedChannels.TryAdd(Guild.Id, AudioClient))
            {
                Console.WriteLine("joined channel");
            }
        }

        public async Task LeaveAudio(IGuild Guild)
        {
            IAudioClient Client;
            if (ConnectedChannels.TryRemove(Guild.Id, out Client))
            {
                await Client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task SendAudioFFMPEGFileAsync(IGuild guild, IMessageChannel channel, string path)
        {
            // Your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }
            IAudioClient AClient;
            if (ConnectedChannels.TryGetValue(guild.Id, out AClient))
            {
                //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
                using (Process ffmpeg = CreateProcessFromFile(path))
                using (AudioOutStream stream = AClient.CreatePCMStream(AudioApplication.Music))
                {
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync(); }
                }
            }
        }



        //public async Task SendAudioAsync(IGuild guild, IMessageChannel channel)
        //{
        //    IAudioClient client;
        //    if (ConnectedChannels.TryGetValue(guild.Id, out client))
        //    {
        //        //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
        //        using (MemoryStream MemoryStream = new MemoryStream(Resources.TestMusic))
        //        using (AudioOutStream Discord = client.CreateOpusStream())
        //        {
        //            MemoryStream.Position = 0;
        //            try { await MemoryStream.CopyToAsync(Discord); }
        //            finally { await Discord.FlushAsync(); }
        //        }
        //    }
        //}

        private Process CreateProcessFromFile(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private void CreateProcessFromYouTube(string Link)
        {
            
        }
    }
}
