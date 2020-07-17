using DiscordRPC;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordSwearBot
{
    class DbApi
    {
        private DbConnection conn;
        public DbApi(DbConnection conn)
        {
            this.conn = conn;
        }

        public HashSet<string> LoadDictionary()
        {
            string sql = "SELECT text FROM dictionary;";
            HashSet<string> hashSet = new HashSet<string>();
            using (var cmd = new MySqlCommand(sql, conn.connection))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        hashSet.Add(rdr.GetString(0).ToLower());
                    }
                }
            }
            return hashSet;
        }

        public int GetServerTotal()
        {
            string sql = "SELECT value FROM server_stats WHERE name='total';";
            using MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            return int.Parse(rdr.GetString(0));
        }

        public void SetServerTotal(int total)
        {
            using var cmd = new MySqlCommand();
            cmd.Connection = conn.connection;
            cmd.CommandText = "UPDATE server_stats SET value=" + total.ToString() + " where name='total';";
            cmd.ExecuteNonQuery();
        }

        public void DictAdd(string value)
        {    
            using var cmd = new MySqlCommand();
            cmd.Connection = conn.connection;
            cmd.CommandText = string.Format("INSERT INTO dictionary (text) VALUES ('{0}');", value);
            cmd.ExecuteNonQuery();

        }

        public void DictRm(string value)
        {
            using var cmd = new MySqlCommand();
            cmd.Connection = conn.connection;
            cmd.CommandText = string.Format("DELETE FROM dictionary WHERE text='{0}';", value);
            cmd.ExecuteNonQuery();
        }

        public int UserGetTotal(string userId)
        {
            string sql = "SELECT count FROM user_stats WHERE userid='" + userId + "'";
            using MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                rdr.Close();
                using var cmd2 = new MySqlCommand();
                cmd2.Connection = conn.connection;
                cmd2.CommandText = "INSERT INTO user_stats (userid, count) values ('" + userId + "', '0')";
                cmd2.ExecuteNonQuery();
                return 0;
            }
            return int.Parse(rdr.GetString(0));
        }

        public void UserSetTotal(string userId, int value)
        {
            string sql = "UPDATE user_stats SET count = '" + value + "' WHERE userid='" + userId + "'";
            using var cmd = new MySqlCommand();
            cmd.Connection = conn.connection;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}
