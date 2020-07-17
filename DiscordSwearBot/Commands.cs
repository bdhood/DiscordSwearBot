
using DiscordRPC;
using DiscordSwearBot;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Modes;
using System.Linq;
using Renci.SshNet.Messages;

namespace Discord_Bot.src.Modules.Commands
{
    public class Commands : BaseCommandModule
    {

        [Command("help")]
        public async Task SwearHelp(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("```\n" + Bot.prefix + string.Join(Bot.prefix, 
                new string[] { 
                    "counter [@user]\n", 
                    "counter++\n",
                    "dict add <word>\n", 
                    "dict rm <word>\n", 
                    "dict list\n", 
                    "dict list [n]\n",
                    "help\n",
                }) + "```");
        }

        [Command("counter")]
        [Description("Get the number of swear words said")]
        public async Task SwearBot(CommandContext ctx)
        {
            DbConnection conn = new DbConnection(Bot.dbSettings);
            DbApi dbApi = new DbApi(conn);

            string[] args = ctx.Message.Content.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 1)
            {
                int total = dbApi.GetServerTotal();
                await ctx.Channel.SendMessageAsync("Server Counter: " + total.ToString()).ConfigureAwait(false);
            }
            else if (args.Length == 2)
            {
                if (args[1].StartsWith("<@!") && args[1].EndsWith(">"))
                {
                    int user_total = dbApi.UserGetTotal(args[1]);
                    await ctx.Channel.SendMessageAsync("Counter for " + args[1] + " is " + user_total.ToString()).ConfigureAwait(false);
                }

            }
        }

        [Command("counter++")]
        [Description("Get the number of swear words said")]
        public async Task CounterPP(CommandContext ctx)
        {
            DbConnection conn = new DbConnection(Bot.dbSettings);
            DbApi dbApi = new DbApi(conn);

            int total = dbApi.GetServerTotal();
            total++;
            dbApi.SetServerTotal(total);
            await ctx.Channel.SendMessageAsync("Server Counter: " + total.ToString()).ConfigureAwait(false);
        }

        [Command("counter--")]
        [Description("Get the number of swear words said")]
        public async Task CounterMM(CommandContext ctx)
        {
            DbConnection conn = new DbConnection(Bot.dbSettings);
            DbApi dbApi = new DbApi(conn);

            int total = dbApi.GetServerTotal();
            total--;
            dbApi.SetServerTotal(total);
            await ctx.Channel.SendMessageAsync("Server Counter: " + total.ToString()).ConfigureAwait(false);
        }
        [Command("dict")]
        [Description("Adds a new word to the swear dictionary, everything is case insensitive")]
        public async Task Dict(CommandContext ctx)
        {
            DbConnection conn = new DbConnection(Bot.dbSettings);
            DbApi dbApi = new DbApi(conn);
            string[] args = ctx.Message.Content.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if ((args.Length == 2 || args.Length == 3) && args[1] == "list")
            {
                int perPage = 25;
                string[] list = dbApi.LoadDictionary().ToArray();
                int start = 0;
                if (args.Length == 3)
                {
                    start = int.Parse(args[2]) - 1;
                }
                string message = "Swear Dictionary : Page " + (start + 1).ToString() + " of " + ((list.Count() / 25) + 1).ToString() + "\n```\n";
                if (start + 1 > (list.Count() / 25) + 1)
                {
                    return;
                }
                for (int i = start * perPage; i < (start * perPage) + perPage; i++)
                {
                    if (i < list.Length)
                    {
                        message += list[i] + "\n";
                    }
                }
                message += "```";
                await ctx.Channel.SendMessageAsync(message);
            }
            else if (args.Length != 3)
            {
                await ctx.Channel.SendMessageAsync("Invalid format, please try again\nExample:\n.dict [add|rm] <word>\n.dict list <n>");
            }
            else if (args[1] == "rm")
            {
                string word = args[2];
                dbApi.DictRm(word);
                await ctx.Channel.SendMessageAsync("Removed '" + word + "'");
            }
            else
            {
                string word = args[2];
                dbApi.DictAdd(word);
                await ctx.Channel.SendMessageAsync("Added '" + word + "'");
            }
        }
    }
}
