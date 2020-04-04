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
        public static string UtilitiesHelp = "";

        public const string RandomNumberString = "RandomNumber - This command will generate a random number for you\n";
        [Command("randomnumber")]
        [Alias("rn")]
        public async Task RandomNumberAsync()
        {
            await ReplyAsync($"You have too few args.\nThe can be have 'Max' or 'Min' followed by 'Max' all arguments are seperated by spaces.");
        }

        [Command("randomnumber")]
        [Alias("rn")]
        public async Task RandomNumberAsync([Remainder]string Input)
        {
            string[] Inputs = Input.Split(' ');
            if (Inputs.Count() > 2)
            {
                await ReplyAsync($"You have too many args.\nThe can be have 'Max' or 'Min' and 'Max' all arguments are seperated by spaces.");
                return;
            }
            if (Inputs.Count() == 2)
            {
                int Max, Min; 
                bool MaxIsNum = int.TryParse(Inputs[1], out Max);
                bool MinIsNum = int.TryParse(Inputs[0], out Min);
                if(!MaxIsNum || !MinIsNum)
                {
                    await ReplyAsync($"On of your args was not a whole number.\nNote that arguments are only seperated by spaces.");
                    return;
                }
                if(Max < Min)
                {
                    await ReplyAsync($"Your random number is:{StaticServices.RandomN.Next(Max, Min)}");
                    return;
                }

                await ReplyAsync($"Your random number is:{StaticServices.RandomN.Next(Min, Max)}");
                return;
            }
            if (Inputs.Count() == 1)
            {
                int Max;
                bool MaxIsNum = int.TryParse(Inputs[0], out Max);
                if (!MaxIsNum)
                {
                    await ReplyAsync($"On of your args was not a whole number.");
                    return;
                }

                await ReplyAsync($"Your random number is:{StaticServices.RandomN.Next(Max)}");
                return;
            }
            await ReplyAsync($"You have too few args.\nThe can be have 'Max' or 'Min' followed by 'Max' all arguments are seperated by spaces.");
        }

        const string MultiRoll = "Your rolls are the following `[{0}]` for: a `total` of `{1}`; a top value of `{2}`; a `min` value of `{3}`.";
        const string RolledMessage = "You have rolled a";
        const string InvalidModifier = "The modifier you have provided is in valid! use (m+[x] or m-[x])";

        const string D100HiddenHelpString = "D100DiceHidden";
        [Command("d100dicehidden")]
        [Alias("d100h")]
        public async Task D100HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(0,99)}||");
        }

        [Command("d20dicehidden")]
        [Alias("d20h")]
        public async Task D20HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 20)}||");
        }

        [Command("d12dicehidden")]
        [Alias("d12h")]
        public async Task D12HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 12)}||");
        }

        [Command("d10dicehidden")]
        [Alias("d10h")]
        public async Task D10HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(0, 9)}||");
        }

        [Command("d8dicehidden")]
        [Alias("d8h")]
        public async Task D8HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 8)}||");
        }

        [Command("d6dicehidden")]
        [Alias("d6h")]
        public async Task D6HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 6)}||");
        }

        [Command("d4dicehidden")]
        [Alias("d4h")]
        public async Task D4HAsync([Remainder]string Input)
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 4)}||");
        }

        [Command("flipcoinhidden")]
        [Alias("fch")]
        public async Task FlipCoinHAsync([Remainder]string Input)
        {
            string Result;
            int CoinNum = StaticServices.RandomN.Next(100);
            if (CoinNum < 50)
                Result = "heads";
            else
                Result = "tails";
            await ReplyAsync($"You have fliped a ||{Result}||");
        }

        [Command("d100dice")]
        [Alias("d100")]
        public async Task D100Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(0, 99)}");
        }

        [Command("d100dice")]
        [Alias("d100")]
        public async Task D100Async(string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int Value = StaticServices.RandomN.Next(0, 99);
            await ReplyAsync($"{RolledMessage} {Value} for a {Value + Moddifier}");
        }

        [Command("d100dice")]
        [Alias("d100")]
        public async Task D100Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(0, 99);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d100dice")]
        [Alias("d100")]
        public async Task D100Async(int NumberOfRolls, string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(0, 99);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        [Command("d20dice")]
        [Alias("d20")]
        public async Task D20Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(1, 20)}");
        }

        [Command("d20dice")]
        [Alias("d20")]
        public async Task D20Async(string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int Value = StaticServices.RandomN.Next(1, 20);
            await ReplyAsync($"{RolledMessage} {Value} for a {Value + Moddifier}");
        }

        [Command("d20dice")]
        [Alias("d20")]
        public async Task D20Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 20);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d20dice")]
        [Alias("d20")]
        public async Task D20Async(int NumberOfRolls, string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 20);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        [Command("d12dice")]
        [Alias("d12")]
        public async Task D12Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(1, 12)}");
        }

        [Command("d12dice")]
        [Alias("d12")]
        public async Task D12Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 12);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d10dice")]
        [Alias("d10")]
        public async Task D10Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(1, 10)}");
        }

        [Command("d10dice")]
        [Alias("d10")]
        public async Task D10Async(string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int Value = StaticServices.RandomN.Next(1, 10);
            await ReplyAsync($"{RolledMessage} {Value} for a {Value + Moddifier}");
        }

        [Command("d10dice")]
        [Alias("d10")]
        public async Task D10Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 10);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d10dice")]
        [Alias("d10")]
        public async Task D10Async(int NumberOfRolls, string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 10);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        [Command("d8dice")]
        [Alias("d8")]
        public async Task D8Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(1, 8)}");
        }

        [Command("d8dice")]
        [Alias("d8")]
        public async Task D8Async(string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int Value = StaticServices.RandomN.Next(1, 8);
            await ReplyAsync($"{RolledMessage} {Value} for a {Value + Moddifier}");
        }

        [Command("d8dice")]
        [Alias("d8")]
        public async Task D8Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 8);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d8dice")]
        [Alias("d8")]
        public async Task D8Async(int NumberOfRolls, string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 8);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        [Command("d6dice")]
        [Alias("d6")]
        public async Task D6Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(1, 6)}");
        }

        [Command("d6dice")]
        [Alias("d6")]
        public async Task D6Async(string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int Value = StaticServices.RandomN.Next(1, 6);
            await ReplyAsync($"{RolledMessage} {Value} for a {Value + Moddifier}");
        }

        [Command("d6dice")]
        [Alias("d6")]
        public async Task D6Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 6);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d6dice")]
        [Alias("d6")]
        public async Task D6Async(int NumberOfRolls, string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 6);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        [Command("d4dice")]
        [Alias("d4")]
        public async Task D4Async()
        {
            await ReplyAsync($"{RolledMessage} {StaticServices.RandomN.Next(1, 4)}");
        }

        [Command("d4dice")]
        [Alias("d4")]
        public async Task D4Async(string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int Value = StaticServices.RandomN.Next(1, 4);
            await ReplyAsync($"{RolledMessage} {Value} for a {Value + Moddifier}");
        }

        [Command("d4dice")]
        [Alias("d4")]
        public async Task D4Async(int NumberOfRolls)
        {
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 4);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum(), Rolls.Max(), Rolls.Min()));
        }

        [Command("d4dice")]
        [Alias("d4")]
        public async Task D4Async(int NumberOfRolls, string Modifier)
        {
            int Moddifier;
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    Moddifier = int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    Moddifier = -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    await ReplyAsync(InvalidModifier);
                    return;
                }
            }
            else
            {
                await ReplyAsync(InvalidModifier);
                return;
            }
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 4);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        [Command("flipcoin")]
        [Alias("fc")]
        public async Task FlipCoinAsync()
        {
            string Result;
            int CoinNum = StaticServices.RandomN.Next(100);
            if (CoinNum < 50)
                Result = "heads";
            else
                Result = "tails";
            await ReplyAsync($"You have fliped a {Result}");
        }
    }
}
