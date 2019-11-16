using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.Commands;
using DiscordBot.Services;
using DiscordBot.Properties;

namespace DiscordBot.Modules
{
    public class BasicCommandModule : ModuleBase<SocketCommandContext>
    {
        public static string HelpString { get; private set; } = $"🗺️_**For Basic Commands:**_🗺️\n```{PingHelpString}{EcoHelpString}{InviteHelpString}```";

        [Command("help")]
        [Alias("h")]
        public async Task HelpAsync()
        {
            await ReplyAsync(HelpString);
        }

        public const string PingHelpString = "Ping - This command will return an approximation of your ping.\n";
        [Command("ping")]
        [Alias("pong", "p")]
        public async Task PingAsync()
        {
            await ReplyAsync($"Pong ⌛{DateTime.UtcNow.Second * 1000 + DateTime.UtcNow.Millisecond - Context.Message.CreatedAt.ToUniversalTime().Second * 1000 + Context.Message.CreatedAt.ToUniversalTime().Millisecond - Context.Client.Latency} ms!");
        }

        public const string EcoHelpString = "Eco - This command will eco that you typed in.\n";
        [Command("eco")]
        [Alias("e")]
        public async Task EcoAsync()
        {
            await ReplyAsync($"Ecooo... :\n```{ Context.Message }```");
        }

        public const string InviteHelpString = "Invite - This command will generate an invite link for you to use to add me to your servers\n";
        [Command("invite")]
        [Alias("i")]
        public async Task InviteAsync()
        {
            RestApplication Info = await Context.Client.GetApplicationInfoAsync();
            string link = $"https://discordapp.com/api/oauth2/authorize?client_id={ Info.Id }&permissions=8&scope=bot";
            await Context.Channel.SendPhotoAsync(Resources.TimeToFly, "TimeToFly", $"Oh thanks for wanting to invite me into another server! 💞\nMy here is my link: { link } ");
        }
    }
}
