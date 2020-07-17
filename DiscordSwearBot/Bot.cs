using Discord_Bot.src.Modules.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace DiscordSwearBot
{
    public class Bot
    {

        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public static DbSettings dbSettings { get => _dbSettings; }
        private static DbSettings _dbSettings;
        public static string prefix;

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<Configjson>(json);

            var botkey = Environment.GetEnvironmentVariable("DISCORD_SWEAR_BOT_TOKEN");
            if (string.IsNullOrEmpty(botkey)) { 
                throw new Exception("Environment variable DISCORD_SWEAR_BOT_TOKEN is not set!");
            }

            string mysql_password = Environment.GetEnvironmentVariable("DISCORD_SWEAR_BOT_DB_PASSWORD");
            if (string.IsNullOrEmpty(mysql_password))
            {
                throw new Exception("Environment variable DISCORD_SWEAR_BOT_DB_PASSWORD is not set!");
            }

            _dbSettings = configJson.dbSettings;
            _dbSettings.password = mysql_password;

            var config = new DiscordConfiguration
            {
                Token = botkey,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.MessageCreated += Client_MessageCreated;
            Client.MessageReactionAdded += Client_MessageReactionAdded;
            if (configJson.Prefix.Length > 0)
            {
                prefix = configJson.Prefix[0];
            }

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = configJson.Prefix,
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false,
                IgnoreExtraArguments = true,
            };
            Commands = Client.UseCommandsNext(commandsConfig);

            //REGRISTRATION FOR COMMAND FILES:
            Commands.RegisterCommands<Commands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task Client_MessageReactionAdded(MessageReactionAddEventArgs e)
        {
            return Task.CompletedTask;
        }

        private Task Client_MessageCreated(MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            DbConnection conn = new DbConnection(Bot.dbSettings);
            DbApi dbApi = new DbApi(conn);
            HashSet<string> hashSet = dbApi.LoadDictionary();

            List<string> swearWords = new List<string>();
            foreach (string word in e.Message.Content.Split(new char[] { ' ', ',', ';', '&', '-', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (hashSet.Contains(word.ToLower()))
                {
                    swearWords.Add(word);
                }
            }

            if (swearWords.Count == 0)
            {
                return Task.CompletedTask;
            }

            int total = dbApi.GetServerTotal();
            total += swearWords.Count;
            dbApi.SetServerTotal(total);

            int user_total = dbApi.UserGetTotal(e.Author.Mention);
            dbApi.UserSetTotal(e.Author.Mention, user_total + swearWords.Count);

            return e.Channel.SendMessageAsync("lmao " + e.Author.Mention + " just said \"" + string.Join("\", \"", swearWords.ToArray()) + "\"");
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
