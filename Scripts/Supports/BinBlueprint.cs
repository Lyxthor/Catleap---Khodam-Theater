using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinBlueprint
{
    private List<string> columns;
    public void Add(string col)
    {
        columns.Add(col);
    }
    /*public void Create()
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/{FileName}.bin";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    private List<Dictionary<string, object>> CreateCols()
    {
        List<Dictionary<string, object>> cols = new List<Dictionary<string, object>>();

    }*/
}
