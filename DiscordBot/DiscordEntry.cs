using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord.API;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Commands.Builders;
using System.Reflection;
using DiscordBot.Modules;
using DiscordBot.Services;

namespace DiscordBot
{
    public class DiscordClient : IDisposable
    {
        const string CommandKey = "~";

        string BotToken;

        DiscordSocketClient Client;

        CommandService Command;

        AudioService Audio;

        YoutubeService Youtube;

        IServiceProvider Services;

        public Action<LogMessage> LogAction = (x) => { Console.WriteLine(x.ToString()); };

        public Action<DiscordSocketClient> ConnectAction = (x) => { Console.WriteLine($"{x.CurrentUser} is connected!"); };

        public Task HoldingTask { private set; get; }

        public DiscordClient(string BotToken)
        {
            this.BotToken = BotToken;
            Client = new DiscordSocketClient();
            Command = new CommandService();
            Audio = new AudioService();
            Youtube = new YoutubeService();

            Client.Log += LogAsync;
            Client.MessageReceived += MessageReceivedAsync;

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(Command)
                .AddSingleton(Audio)
                .AddSingleton(Youtube)
                .BuildServiceProvider();
        }

        public async Task MainStartAsync()
        {
            await RegisterCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, BotToken);
            await Client.StartAsync();
            HoldingTask = Task.Delay(-1);
            await HoldingTask;
        }

        public async Task RegisterCommandsAsync()
        {
            // Tokens should be considered secret data, and never hard-coded.
            await Command.AddModulesAsync(Assembly.GetExecutingAssembly(), Services);
        }

        private Task LogAsync(LogMessage Log)
        {
            LogAction.Invoke(Log);
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            ConnectAction.Invoke(Client);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage RawMessage)
        {
            const string Source = "Command Module";

            SocketUserMessage Message = RawMessage as SocketUserMessage;

            if (Message == null || Message.Author.IsBot || Message.Author.Id == Client.CurrentUser.Id)
                return;

            int ArgPos = 0;

            if (Message.HasStringPrefix(CommandKey, ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))
            {
                try
                {
                    SocketCommandContext Context = new SocketCommandContext(Client, Message);

                    LogAction.Invoke(new LogMessage(LogSeverity.Info, Source, $"Command Model Called:{Message}"));

                    IResult Result = await Command.ExecuteAsync(Context, ArgPos, Services);

                    if (!Result.IsSuccess)
                    {
                        if (Result.Error == CommandError.UnknownCommand)
                        {
                            LogAction(new LogMessage(LogSeverity.Info, Source, $"{Result.Error}:{Result.ErrorReason}"));
                            await Context.Channel.SendMessageAsync($"Command Not Found 🔍\n**\"I dont follow, but its not a problem!\"**\nType {CommandKey}Help or {CommandKey}H for help 📚!");
                        }
                        else if (Result.Error == CommandError.UnmetPrecondition)
                        {
                            if (Result.ErrorReason.Contains(AdminCommandModule.PermError))
                            {
                                LogAction(new LogMessage(LogSeverity.Info, Source, $"{Result.Error}:{Result.ErrorReason}"));
                                await Context.Channel.SendMessageAsync($"It seem do not have clearance for this command 🚨\n**\"What are you trying to pull here!\"**\nType {CommandKey}Help or {CommandKey}H for help 📚!");
                            }
                            else
                            {
                                LogAction(new LogMessage(LogSeverity.Info, Source, $"{Result.Error}:{Result.ErrorReason}"));
                                await Context.Channel.SendMessageAsync($"It there is a condition not met 🚨\n**\"What are you trying to pull here!\"**\nType {CommandKey}Help or {CommandKey}H for help 📚!");
                            }
                        }
                        else if (Result.Error == CommandError.Exception)
                        {
                            LogAction(new LogMessage(LogSeverity.Error, Source, $"{Result.Error}:{Result.ErrorReason}"));
                            await Context.Channel.SendMessageAsync($"Oppps there seem so have been an internal error ⚙️\n**\"Urzu 7 under fire, but its not a problem!\"**\nYour issue has been logged 📚, Please return to the fight and try again!\nFailing that give us time to look into the issue.");
                        }
                    }
                }
                catch (Exception e)
                {
                    LogAction.Invoke(new LogMessage(LogSeverity.Error, Source, e.Message, e));
                }
            }
                
                
        }

        public void Dispose()
        {
            Client?.Dispose();
        }

        ~DiscordClient()
        {
            try
            {
                Client?.Dispose();
            }
            catch { }
        }
    }
}
