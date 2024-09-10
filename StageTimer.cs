using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StageTimer : MonoBehaviour
{
    public GameObject stage;
    public Button stageButton;
    public StageStoryView view;
    public int interval;
    private TimerController localTimerController;
    private StageTimeModel model;
    private Dictionary<string, object> data;
    private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    private void Awake()
    {
        model = new StageTimeModel();
        localTimerController = GetComponent<TimerController>();
        GetData();
        
    }
    public void GetData()
    {
        Debug.Log("Timer test");
        data = model.Get()[0];
        SetTimer();
       // SetTimer();
    }
    
    private int GetTimeDiffs()
    {
        DateTime lastPaymentTime = DateTime.ParseExact((string)data["last_time"], dateTimeFormat, null);
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDifferences = currentTime - lastPaymentTime;
        return (int)timeDifferences.TotalSeconds;
    }
    private void SetTimer()
    {
        int timeDiff = GetTimeDiffs();
        if (timeDiff > (int)data["interval"])
        {
            Debug.Log("Time is greater" + timeDiff);
            interval = 0;
            localTimerController.SetData(interval, EnableStage);
            ToggleStage(true);
        }
        else
        {
            Debug.Log("Time is lesser" + timeDiff);
            interval = (int)data["interval"] - timeDiff;
            localTimerController.SetData(interval, EnableStage);
            ToggleStage(false);
        }

    }
    public void SetCurrentTime()
    {
        data["last_time"] = DateTime.Now.ToString(dateTimeFormat);
        model.CreateOrUpdate(new List<Dictionary<string, object>> { data });
        GetData();
    }
    private void ToggleStage(bool state)
    {
        StageInfoCourier.Instance.isStageEnabled = state;
        view.EnableView(state);
        localTimerController.SetPaused(state);
    }
    private void EnableStage()
    {
        ToggleStage(true);
    }
    
}
