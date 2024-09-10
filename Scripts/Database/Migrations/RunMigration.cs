using Connections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class RunMigration : MonoBehaviour
{
    private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private void Awake()
    {
        
        /*PlayzoneTable.Up();
        StandTable.Up();
        StandDialogueTable.Up();
        PropertyDialogueTable.Up();*/
        PaymentSeeder();
        StatsSeeder();
        BallSeeder();
        HighestScoreSeeder();
        StageTimeSeeder();
        /*PaymentBinTable.Up();
        StatsBinTable.Up();*/
        
    }
    private void PaymentSeeder()
    {
        PaymentModel model = new PaymentModel();
        
        if (model.IsExists()) return;
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        Dictionary<string, object> entry = new Dictionary<string, object>();
        entry.Add("interval", 30);
        entry.Add("expenses", 0);
        entry.Add("disabled", false);
        entry.Add("last_payment_time", DateTime.Now.ToString(dateTimeFormat));
        data.Add(entry);
        model.CreateOrUpdate(data);
    }
    private void StatsSeeder()
    {
        StatsModel model = new StatsModel();
        
        if (model.IsExists()) return;
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        Dictionary<string, object> entry = new Dictionary<string, object>();
        entry.Add("exp", 900);
        entry.Add("aura", 1000);
        entry.Add("level", 1);
        entry.Add("ticket", 0);
        entry.Add("targetExp", 1000);
        entry.Add("ball_amount", 0);
        data.Add(entry);
        model.CreateOrUpdate(data);
    }
    private void BallSeeder()
    {
        BallModel model = new BallModel();
        
        if (model.IsExists()) return;
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        Dictionary<string, object> entry = new Dictionary<string, object>();
        entry.Add("amount", 0);
        entry.Add("price", 1000);
        data.Add(entry);
        model.CreateOrUpdate(data);
    }
    private void HighestScoreSeeder()
    {
        HighestScoreModel model = new HighestScoreModel();
        
        if (model.IsExists()) return;
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        Dictionary<string, object> entry = new Dictionary<string, object>();
        entry.Add("ryhtm_score", 0);
        entry.Add("coloring_score", 0);
        entry.Add("baloon_score", 0);
        entry.Add("race_score", 0);
        data.Add(entry);
        model.CreateOrUpdate(data);
    }
    private void StageTimeSeeder()
    {
        StageTimeModel model = new StageTimeModel();
        if (model.IsExists()) return;
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        Dictionary<string, object> entry = new Dictionary<string, object>();
        entry.Add("interval", 60);
        entry.Add("last_time", DateTime.Now.ToString(dateTimeFormat));
        data.Add(entry);
        model.CreateOrUpdate(data);
    }
    private void OnApplicationQuit()
    {
        SqliteDbConnection.Disconnect();
    }
}
