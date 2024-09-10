using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Events;

public class StandController : MonoBehaviour
{
    public Transform collectAuraPoint;
    public StatsController globalStatsController;
    private StandView view;
    private StandModel model;

    private Coroutine getIncome=null;
    private Coroutine collectAura = null;
    private CollectAuraButtonController auraButtonController;
    private PropertyDialogueController dialogueController;

    private int accumulatedIncome;
    private int entryId;
    private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private List<Dictionary<string, object>> dialogueData;
    private Dictionary<string, object> data;

    private void Start()
    {
       
    }
    private void OnEnable()
    {
        EventRegister();
    }
    public void EventRegister()
    {
        EventList.OnPurchasedStand.Subscribe(PurchaseStand);
        EventList.OnUpgradeStand.Subscribe(UpgradeStand);
    }
    public void GetData()
    {
        data = model.Get(entryId)[0];
        RenderView();
    }
    private void UpdateData()
    {
        model.Update(data, entryId);
    }
    private void PurchaseStand(int id)
    {
        if (data == null || id != entryId) return;
        data = model.Get(entryId)[0];
        RenderView();
    }
    private void UpgradeStand(int id)
    {
        if (data == null || id != entryId) return;
        data = model.Get(entryId)[0];
        RenderView();
    }
    public void SetDependencies(Dictionary<string, object> _data, StandModel _model, StatsController _globalStatsController, Transform _targetPoint) {
        Init();
        
        model = _model;
        data = _data;

        globalStatsController = _globalStatsController;
        collectAuraPoint = _targetPoint;

        if (data == null) return;
        if (data.Count == 0) return;
        entryId = (int)(long)data["id"];
        if((string) data["active_state"]=="unlocked") CalculateInactiveIncome();
        bool activeState = (string)data["active_state"] == "unlocked";
        dialogueController.SetPropertyData((int)(long)data["id"], "Stand", activeState);
        RenderView();
    }
    private void Init()
    {
        if (data != null) return;
        view = GetComponent<StandView>();
        auraButtonController = view.collectAuraButton.GetComponent<CollectAuraButtonController>();
        dialogueController = GetComponent<PropertyDialogueController>();
        
        
        view.collectAuraButton.onClick.AddListener(StartMoveAuraButton);
        
        EventRegister();
    }
    private void RenderView()
    {
        view.Render(data);
        if (getIncome == null&&(string) data["active_state"] == "unlocked")
            getIncome = StartCoroutine(TimerController.SetInterval((int)(long)data["income_interval"], GetIncome));
    }
    private void StartMoveAuraButton()
    {
        view.ShowUpdateText(accumulatedIncome.ToString());
        data["last_get_income_time"]=DateTime.Now.ToString(dateTimeFormat);
        auraButtonController.target = collectAuraPoint.position;
        auraButtonController.enabled = true;
        auraButtonController.collectAura = CollectIncome;
        UpdateData();
    }
    private void CalculateInactiveIncome()
    {
        string lastGetIncomeTime = (string)data["last_get_income_time"];
        if (lastGetIncomeTime == "") return;
        int timeDifferences = CalculateTimeDifferences(lastGetIncomeTime);
        int incomeInterval = (int)(long)data["income_interval"];
        int remainTime = timeDifferences % incomeInterval;
        int getAmount = timeDifferences / incomeInterval;
        if(getAmount > 0)
        {
            int income = CalculateIncome();
            accumulatedIncome += income * getAmount;
            view.ShowCollectAuraButton();
            getIncome = StartCoroutine(TimerController.SetTimeout(remainTime, StartGetIncomeFromRemainTime));
        } else
        {
            data["last_get_income_time"] = DateTime.Now.ToString(dateTimeFormat);
            model.Update(data, (int)(long)data["id"]);
        }
        
    }
    private int CalculateTimeDifferences(string _lastGetIncomeTime)
    {
        DateTime lastGetIncomeTime = DateTime.ParseExact(_lastGetIncomeTime, dateTimeFormat, null);
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDifferences = currentTime - lastGetIncomeTime;
        return (int)timeDifferences.TotalSeconds;

    }
    private void StartGetIncomeFromRemainTime()
    {
        GetIncome();
        getIncome = StartCoroutine(TimerController.SetInterval((int)(long)data["income_interval"], GetIncome));
    }
    private void GetIncome()
    {
        
        int income = CalculateIncome();
        accumulatedIncome += income;
        view.ShowCollectAuraButton();
        
    }
    private int CalculateIncome()
    {
        int level = (int)(long)data["level"];
        int price = (int)(long)data["price"];
        return StatsController.GetIncome(level, price);
    }
    private void CollectIncome()
    {
        globalStatsController.UpdateAura(accumulatedIncome);
        accumulatedIncome = 0;
    }
    private void OnDisable()
    {
        EventList.OnPurchasedStand.Unsubscribe(PurchaseStand);
        EventList.OnUpgradeStand.Unsubscribe(UpgradeStand);
    }
    private void OnDestroy()
    {
        EventList.OnPurchasedStand.Unsubscribe(PurchaseStand);
        EventList.OnUpgradeStand.Unsubscribe(UpgradeStand);
    }
}
