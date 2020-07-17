# Discord Swear Bot

### Configuration
1. Setup a mysql database. Probably others will work as the sql is simple
2. Get a discord bot token
3. Enter required environment variables
	- `DISCORD_SWEAR_BOT_TOKEN` token generated from discord
	- `DISCORD_SWEAR_BOT_DB_PASSWORD` password for mysql connect
4. Run the .sql file in the project root. It creates the db structure and swear dictionary
5. Open the .sln and run!

### Bot Commands

| Command  |  Description |
| :------------ | :------------ |
| `^counter [@mention] ` | Get the swear count of the server or a particular user  |
| `^counter++`  | Mainly for laughs, just increments the server counter  |
| `^dict add <word>`  | Adds a word to the dictionary  |
| `^dict rm <word>` | Remove a word from the dictionary  |
|  `^dict list [page]`  | Lists the dictionary, optional page number  |
| `^help` | Basically what you are reading now |
