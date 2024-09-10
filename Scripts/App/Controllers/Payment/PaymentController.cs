using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PaymentController : MonoBehaviour
{
    public StatsController globalStatsController;
    private Dictionary<string, object> data;
    private PaymentModel model;
    private TimerController localTimerController;
    private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private void Awake()
    {
        model = new PaymentModel();
       /* if (!model.IsExists()) SetDummyData();*/
        localTimerController = GetComponent<TimerController>();
        GetData();
        
    }
    
    private void GetData()
    {
        if(model==null)
        {
            model = new PaymentModel();
            localTimerController = GetComponent<TimerController>();
        }
        if(data==null)
        {
            data = model.Get()[0];
            CalculateInactiveExpenses();
        }
    }
    public int GetExpenses()
    {
        return (int)data["expenses"];
    }
    public void UpdateData(Dictionary<string, object> _data)
    {
        
        data = _data;
        model.CreateOrUpdate(new List<Dictionary<string, object>> { data });
    }
    public void UpdateExpenses(int _expenses)
    {
        GetData();
        data["expenses"] = _expenses;
        model.CreateOrUpdate(new List<Dictionary<string, object>> { data });
    }
    private void CalculateInactiveExpenses()
    {
        if ((string)data["last_payment_time"] == "")
        {
            localTimerController.SetData((int)data["interval"], PayExpenses);
            return;
        }
        int timeDifferences = GetTimeDifferences();
        int paymentInterval = (int)data["interval"];
        int paymentAmountChances = timeDifferences / paymentInterval;
        int remainTime = timeDifferences % paymentInterval;
        if(remainTime > 0)
        {
            int expenses = (int)data["expenses"];
            expenses *= paymentAmountChances;
            globalStatsController.UpdateAura(-expenses);
            data["last_payment_time"] = DateTime.Now.ToString(dateTimeFormat);
            model.CreateOrUpdate(new List<Dictionary<string, object>> { data });
            localTimerController.SetData(remainTime, PayExpensesFromRemainTime);
            return;
        }
        localTimerController.SetData((int)data["interval"], PayExpenses);
    }
    private void PayExpensesFromRemainTime()
    {
        PayExpenses();
        localTimerController.SetData((int)data["interval"], PayExpenses);
    }
    private int GetTimeDifferences()
    {
        DateTime lastPaymentTime = DateTime.ParseExact((string)data["last_payment_time"], dateTimeFormat, null);
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDifferences = currentTime - lastPaymentTime;
        return (int) timeDifferences.TotalSeconds;
    }
    private void PayExpenses()
    {
        int expenses = (int)data["expenses"];
        globalStatsController.UpdateAura(-expenses);
        data["last_payment_time"] = DateTime.Now.ToString(dateTimeFormat);
        model.CreateOrUpdate(new List<Dictionary<string, object>> { data });
    }

    private void SetDummyData()
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        Dictionary<string, object> entry = new Dictionary<string, object>();
        entry.Add("interval", 10);
        entry.Add("expenses", 4000);
        entry.Add("disabled", false);
        entry.Add("last_payment_time", DateTime.Now.ToString(dateTimeFormat));
        data.Add(entry);
        model.CreateOrUpdate(data);
    }
}
