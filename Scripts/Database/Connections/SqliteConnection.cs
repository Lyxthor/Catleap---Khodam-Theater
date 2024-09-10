using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;

namespace Connections
{
    public class SqliteDbConnection
    {
        private static string DBname = "application.db";
        private static string DBPath = $"Data Source={Path.Combine(Application.persistentDataPath, DBname)};Version=3";
        
        public static SqliteConnection connection;
        public static SqliteConnection connect()
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            SetSources();
            connection = new SqliteConnection(DBPath);
            connection.Open();
            return connection;
        }
        public static void Disconnect()
        {
            connection.Close();
        }
        private static void SetSources()
        {
            string persistentPath = Path.Combine(Application.persistentDataPath, DBname);
            string streamingPath = Path.Combine(Application.streamingAssetsPath, DBname);
            if (!File.Exists(persistentPath))
            {
                File.Copy(streamingPath, persistentPath, true);
            }
        }
    }

}