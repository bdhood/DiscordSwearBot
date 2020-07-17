using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordSwearBot
{
    public class DbConnection
    {
        private DbSettings settings;
        public MySqlConnection connection = null;

        public DbConnection(DbSettings settings)
        {
            this.settings = settings;
            this.Connect();
        }

        public void Connect()
        {
            string connectionString = String.Format(
                "Server={0};Port={1};database={2};Uid={3};Pwd={4}",
                settings.address,
                settings.port,
                settings.database,
                settings.username,
                settings.password);
            this.connection = new MySqlConnection(connectionString);
            connection.Open();
        }
    }
}
