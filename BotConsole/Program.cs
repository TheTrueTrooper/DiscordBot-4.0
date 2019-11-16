using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot;
using System.IO;

namespace BotConsole
{

    class Program
    {
        static void Main(string[] args) => new ProgramStarter().Start().GetAwaiter().GetResult();

        class ProgramStarter
        {
#if DEBUG
            string TokenPath = Environment.CurrentDirectory + "\\_DebugToken.key.txt";
#else
            string TokenPath = Environment.CurrentDirectory + "\\_Token.key.txt";
#endif

            public async Task Start()
            {
                string Token;

                using (StreamReader SR = new StreamReader(TokenPath))
                    Token = SR.ReadLine();

                DiscordClient Client = new DiscordClient(Token);
                await Client.MainStartAsync();
            }
        }
    }

}