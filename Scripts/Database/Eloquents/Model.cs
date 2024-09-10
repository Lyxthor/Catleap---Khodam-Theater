using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Mono.Data.Sqlite;

public abstract class BinaryModel
{
    protected virtual string FileName { get; private set; }
    public bool IsExists()
    {
        
        string path = Path.Combine(Application.persistentDataPath, $"{FileName}.bin");
        return File.Exists(path);
    }
    public void CopyFromStream()
    {
        string path = Path.Combine(Application.persistentDataPath, $"{FileName}.bin");
        string streaming = Path.Combine(Application.streamingAssetsPath, $"{FileName}.bin");
        File.Copy(streaming, path);
    }
    public void CreateOrUpdate(List<Dictionary<string, object>> data)
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/{FileName}.bin";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public List<Dictionary<string, object>> Get()
    {
        string path = $"{Application.persistentDataPath}/{FileName}.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<Dictionary<string, object>> data = formatter.Deserialize(stream) as List<Dictionary<string, object>>;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Specified file does not exists");
            return new List<Dictionary<string, object>>();
        }
    }
}
public class SqliteModel
{
    protected static SqliteConnection connection = Connections.SqliteDbConnection.connect();
    protected virtual string Table { get; }
    protected virtual string primaryKey { get { return "id"; } }

    private string concateKeyValuePair(Dictionary<string, object> data)
    {
        List<string> concatedKeyValues = new List<string>();
        foreach (KeyValuePair<string, object> entry in data)
            concatedKeyValues.Add($"{entry.Key}='{entry.Value}'");
        return string.Join(", ", concatedKeyValues);
    }
    public bool Exists(string Table)
    {
        return false;
    }
    public List<Dictionary<string, object>> All()
    {
        string sql = $"SELECT * FROM {Table}";
        SqliteCommand command = new SqliteCommand(sql, connection);
        //command.ExecuteNonQuery();
        SqliteDataReader reader = command.ExecuteReader();
        return FetchResult(reader);
    }

    public List<Dictionary<string, object>> Where(string col, object value)
    {
        string sql = $"SELECT * FROM {Table} WHERE {col}='{value}'";
        SqliteCommand command = new SqliteCommand(sql, connection);
        SqliteDataReader reader = command.ExecuteReader();
        return FetchResult(reader);
    }
    public List<Dictionary<string, object>> Get(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE {primaryKey}={id}";
        SqliteCommand command = new SqliteCommand(sql, connection);
        SqliteDataReader reader = command.ExecuteReader();
        return FetchResult(reader);
    }
    public void Create(Dictionary<string, string> data)
    {
        if (data.Count != 0)
        {
            List<string> cols = new List<string>(data.Keys);
            List<string> vals = new List<string>(data.Values);
            string sql = $"INSERT INTO {Table} ({string.Join(", ", cols)}) VALUES ({string.Join(", ", vals)})";
            SqliteCommand command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
    public void Update(Dictionary<string, object> data, int id)
    {
        string sql = $"UPDATE {Table} SET {concateKeyValuePair(data)} WHERE {primaryKey} = {id}";
        SqliteCommand command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();
    }
    protected List<Dictionary<string, object>> FetchResult(SqliteDataReader reader)
    {
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
        while (reader.Read())
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
                row.Add(reader.GetName(i), reader.GetValue(i));
            result.Add(row);
        }
        return result;
    }
}
/*public class Model
{
    protected virtual string TableName { get; private set; }
    public BinaryModel Binary;
    public JsonModel JSON;
    public Model() 
    {
        Binary = new BinaryModel(TableName);
        JSON = new JsonModel();
    }
}*/
