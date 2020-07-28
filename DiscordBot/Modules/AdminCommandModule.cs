using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using DiscordBot.Services;
using DiscordBot.Properties;

namespace DiscordBot.Modules
{
    [RequireContext(ContextType.Guild)]
    // make sure the user invoking the command can ban
    [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = PermError + "You Require Ban Permissions for this command!")]
    // make sure the bot itself can ban
    [RequireBotPermission(GuildPermission.Administrator, ErrorMessage = PermError + "The Bot Require Ban Permissions for this command!")]
    public class AdminCommandModule : ModuleBase<SocketCommandContext>
    {
        public const string InfoTag = "Admin";

        public const string EmojiTag = "⚒️";

        public static string Help = $"{EmojiTag}_**For {InfoTag} Commands:**_{EmojiTag}\n```{InfoTag} commands are as follows:{BanHelp}{BroadcastHelp}{BBroadcastHelp}```\nYou will need admin privilege to use this to prevent greaving";

        public const string PermError = "You do not have the required Permissions!\n";

        public const string BanHelp = "\n-Ban will ban the user that you specify after the command. After that you may leave a message. Call with ~ban [User] {optional string:Reason}";
        [Command("ban")]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string Reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: $"{user} was banded by {Context.User} because{Reason}");
            if (Reason == null)
                Reason = ": No reason was provided";
            else
                Reason = $": {Reason}";
            await Context.Channel.SendPhotoAsync(Resources.SouskeCamo, "TakeCareOfThis", $"ok! Let Me deal with the Issue!\nBanning user:{user} was banded by {Context.User} because{Reason}");
        }

        public const string BroadcastHelp = "\n-Broadcast will send your message to all other channels. Call with ~broadcast [string:Message]";
        [Command("broadcast")]
        public async Task BroadcastAsync([Remainder] string Message)
        {
            foreach(ITextChannel Channel in Context.Guild.TextChannels)
            {
                await Channel.SendMessageAsync(Message);
            }
        }

        public const string BBroadcastHelp = "\n-BoldBroadcast(BBroadcastAsync) will send your message to all other channels with bolding markups to wrap it. Call with ~bbroadcast [string:Message]";
        [Command("bbroadcast")]
        public async Task BBroadcastAsync([Remainder] string Message)
        {
            foreach (ITextChannel Channel in Context.Guild.TextChannels)
            {
                await Channel.SendMessageAsync($"**{Message}**");
            }
        }
    }
}
