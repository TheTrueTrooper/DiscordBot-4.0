using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.Commands;
using DiscordBot.Services;
using System.Text.RegularExpressions;

namespace DiscordBot.Modules
{
    public class DiceCommandModule : ModuleBase<SocketCommandContext>
    {
        public const string InfoTag = "Dice";

        public const string EmojiTag = "🧙";

        public static string Help { get => $"{EmojiTag}_**For {InfoTag} Commands:**_{EmojiTag}\n```{InfoTag} commands are as follows:{RollDiceString}{RandomNumberString}{D100DiceHiddenHelpString}{ShortDiceHiddenHelper}{D100DiceHelpString}{ShortDiceHelper}{FlipCoinHiddenHelpString}{FlipCoinHelpString}```"; }

        public static string HelpLong { get => $"{EmojiTag}_**For {InfoTag} Commands:**_{EmojiTag}\n```{InfoTag} commands are as follows:{RollDiceString}{RandomNumberString}{D100DiceHiddenHelpString}{D20DiceHiddenHelpString}{D12DiceHiddenHelpString}{D10DiceHiddenHelpString}{D8DiceHiddenHelpString}{D6DiceHiddenHelpString}{D4DiceHiddenHelpString}{FlipCoinHiddenHelpString}{D100DiceHelpString}{D20DiceHelpString}{D12DiceHelpString}{D10DiceHelpString}{D8DiceHelpString}{D6DiceHelpString}{D4DiceHelpString}{FlipCoinHelpString}```"; }

        public const string ShortDiceHiddenHelper = "\n\tThere is also a ~d20h, ~d12h, ~d10h, ~d8h, ~d6h, and ~d4h variants following the same rules to use.";
        public const string ShortDiceHelper = "\n\tThere is also a ~d20, ~d12, ~d10, ~d8, ~d6, and ~d4 variants following the same rules to use.";

        const string RandomNumberString = "\n-RandomNumber command causes the bot to replay with a random number. command can be called with ~randomnumber or the short hand alias ~rn\n\tThis command requires a tailing number for a max or too to Min and Max in that order";
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

        const string MultiRoll = "Your rolls are the following `[{0}]` for: a `total` of `{1}`; a `top` value of `{2}`; a `min` value of `{3}`.";
        const string RolledMessage = "You have rolled a";
        const string InvalidModifier = "The modifier you have provided is in valid! use (m+[x] or m-[x])";

        const string DiceHiddenHelpString = "\n-{0} command causes the bot to reply with a random number in the range of {1} in a spoiler mark up to hide it. command can be called with ~{2} or the short hand alias ~{3}";
        static string D100DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D100DiceHidden", "0-99", "d100dicehidden", "d100h");

        [Command("d100dicehidden")]
        [Alias("d100h")]
        public async Task D100HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(0,99)}||");
        }

        static string D20DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D20DiceHidden", "1-20", "d20dicehidden", "d20h");
        [Command("d20dicehidden")]
        [Alias("d20h")]
        public async Task D20HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 20)}||");
        }

        static string D12DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D12DiceHidden", "1-12", "d12dicehidden", "d12h");
        [Command("d12dicehidden")]
        [Alias("d12h")]
        public async Task D12HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 12)}||");
        }

        static string D10DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D10DiceHidden", "0-9", "d10dicehidden", "d10h");
        [Command("d10dicehidden")]
        [Alias("d10h")]
        public async Task D10HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(0, 9)}||");
        }

        static string D8DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D8DiceHidden", "1-8", "d10dicehidden", "d8h");
        [Command("d8dicehidden")]
        [Alias("d8h")]
        public async Task D8HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 8)}||");
        }

        static string D6DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D6DiceHidden", "1-6", "d6dicehidden", "d6h");
        [Command("d6dicehidden")]
        [Alias("d6h")]
        public async Task D6HAsync()
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 6)}||");
        }

        static string D4DiceHiddenHelpString = string.Format(DiceHiddenHelpString, "D4DiceHidden", "1-4", "d10dicehidden", "d4h");
        [Command("d4dicehidden")]
        [Alias("d4h")]
        public async Task D4HAsync([Remainder]string Input)
        {
            await ReplyAsync($"{RolledMessage} ||{StaticServices.RandomN.Next(1, 4)}||");
        }

        const string FlipCoinHiddenHelpString = "\n-FlipCoinHidden command causes the bot to reply with a either heads or tails in a spoiler mark up to hide it. command can be called with ~flipcoinhidden or the short hand alias ~fch";
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

        const string DiceHelpString = "\n-{0} command causes the bot to reply with a random number in the range of {1}. command can be called with ~{2} or the short hand alias ~{3}\n\tYou can also optionaly add a number after for the [number] of the dice to roll and a m[+/-Number] ex: ~{3} [NumberOfDice] m[Moddifier]";
        static string D100DiceHelpString = string.Format(DiceHelpString, "D100Dice", "0-99", "d100dice", "d100");
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(0, 99);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        static string D20DiceHelpString = string.Format(DiceHelpString, "D20Dice", "1-20", "d20dice", "d20");
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 20);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        static string D12DiceHelpString = string.Format(DiceHelpString, "D12Dice", "1-12", "d12dice", "d12");
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

        static string D10DiceHelpString = string.Format(DiceHelpString, "D10Dice", "1-10", "d10dice", "d10");
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 10);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        static string D8DiceHelpString = string.Format(DiceHelpString, "D8Dice", "1-8", "d8dice", "d8");
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 8);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        static string D6DiceHelpString = string.Format(DiceHelpString, "D6Dice", "1-6", "d6dice", "d6");
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 6);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        static string D4DiceHelpString = string.Format(DiceHelpString, "D4Dice", "1-4", "d4dice", "d4");
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
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
            int? Moddifier = FindModifier(Modifier);
            if (Moddifier == null)
                return;
            int[] Rolls = new int[NumberOfRolls];
            string RollsListed = "";
            for (int i = 0; i < NumberOfRolls; i++)
                Rolls[i] = StaticServices.RandomN.Next(1, 4);
            foreach (int RandomNumber in Rolls)
                RollsListed += $"{RandomNumber}, ";
            RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
            await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
        }

        const string FlipCoinHelpString = "\n-FlipCoin command causes the bot to reply with a either heads or tails. command can be called with ~flipcoin or the short hand alias ~fc";
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

        int? FindModifier(string Modifier)
        {
            Modifier = Modifier.ToLower();
            if (Modifier.StartsWith("m+"))
            {
                try
                {
                    return int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    ReplyAsync(InvalidModifier);
                    return null;
                }
            }
            else if (Modifier.StartsWith("m-"))
            {
                try
                {
                    return -1 * int.Parse(Modifier.Remove(0, 2));
                }
                catch
                {
                    ReplyAsync(InvalidModifier);
                    return null;
                }
            }
            else
            {
                ReplyAsync(InvalidModifier);
                return null;
            }
        }

        [Command("roll")]
        [Alias("r")]
        public async Task RollDiceGeneric(string Dice, int Moddifier)
        {
            int DiceSize;
            int NumberOfRolls;
            string ErrorMessage;
            if (GetDiceSizeAndNumber(Dice, out NumberOfRolls, out DiceSize, out ErrorMessage))
            {
                int[] Rolls = new int[NumberOfRolls];
                string RollsListed = "";
                for (int i = 0; i < NumberOfRolls; i++)
                    Rolls[i] = StaticServices.RandomN.Next(1, DiceSize);
                foreach (int RandomNumber in Rolls)
                    RollsListed += $"{RandomNumber}, ";
                RollsListed = RollsListed.Remove(RollsListed.Length - 2, 2);
                if(NumberOfRolls > 1)
                    await ReplyAsync(string.Format(MultiRoll, RollsListed, Rolls.Sum() + Moddifier, Rolls.Max() + Moddifier, Rolls.Min() + Moddifier));
                else
                    await ReplyAsync($"{RolledMessage} {Rolls[0]} for a {Rolls[0] + Moddifier}");
            }
            else
                await ReplyAsync(ErrorMessage);
        }

        const string RollDiceString = "\n-RollDice command causes the bot to replay with a roll corrisponding to the number of dice and size of dice specified. Optionally you can add a modifier as well. command can be called with ~rolldice or the short hand alias ~r\n\tThis command requires a tailing fromate of [Number of Dice]d[Dice Size] [Optional modifier].";
        [Command("rolldice")]
        [Alias("r")]
        public async Task RollDiceGeneric(string Dice)
        {
            await RollDiceGeneric(Dice, 0);
        }

        public async Task RollDiceGeneric()
        {
            await ReplyAsync(DiceSizeImproperFormate);
        }

        const string DiceSizeImproperFormate = "The specified dice size is not in a valid for fromate.\nPlease use [Number to Roll]d[Dice to Roll].";
        bool GetDiceSizeAndNumber(string Dice, out int NumberOfRolls, out int DiceSize, out string ErrorMessager)
        {
            //check fromate to start with
            Regex MRegex = new Regex("^[0-9]+[d][0-9]+$");
            Regex SRegex = new Regex("^[d][0-9]+$");
            if (MRegex.IsMatch(Dice))
            {
                string[] Values = Dice.Split('d');
                NumberOfRolls = int.Parse(Values[0]);
                DiceSize = int.Parse(Values[1]);
                ErrorMessager = "";
                return true;
            }

            if (SRegex.IsMatch(Dice))
            {
                string[] Values = Dice.Split('d');
                NumberOfRolls = 1;
                DiceSize = int.Parse(Values[1]);
                ErrorMessager = "";
                return true;
            }

            NumberOfRolls = 0;
            DiceSize = 0;
            ErrorMessager = DiceSizeImproperFormate;
            return false;
        }
        
    }
}
