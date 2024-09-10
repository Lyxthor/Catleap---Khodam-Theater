using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Connections;
public class Schema
{
    public delegate void createSql(Blueprint table);
    public delegate void createBin(BinBlueprint table);
    private static SqliteConnection connection = SqliteDbConnection.connect();
    public static void Create(string tableName, createSql cb)
    {
        Blueprint table = new Blueprint(tableName);
        cb(table);
        SqliteCommand command = new SqliteCommand(table.Query(), connection);
        command.ExecuteNonQuery();
    }
   /* public static void CreateBin(string tableName, createBin cb)
    {
        BinBlueprint table = new BinBlueprint(tableName);
        cb(table);
        table.Create();
    }*/
    public static void Drop(string tableName)
    {

    }
}