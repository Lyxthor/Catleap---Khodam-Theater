using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Events;
using UnityEngine.SceneManagement;

public class StandParentController : MonoBehaviour
{
    public Transform collectAuraPoint;
    public StatsController globalStatsController;
    public PaymentController globalExpensesController;
    public StandController[] childControllers;
    private List<Dictionary<string, object>> data, dialogueData;
    private StandModel model; // THIS MODEL USES SQLITEMODEL, SO BE AWARE OF THE DATA TYPES
    private StandDialogueModel standDialogueModel;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        EventRegister();
        if(data==null)
        {
            model = new StandModel();
            standDialogueModel = new StandDialogueModel();
            GetData();
        }
    }
    private void Start()
    {
        model = new StandModel();
        standDialogueModel = new StandDialogueModel();
        GetData();
    }
    private void EventRegister()
    {
        EventList.OnPurchasedStand.Subscribe(SetExpenses);
        EventList.OnUpgradeStand.Subscribe(SetExpenses);
    }
    private void GetData()
    {
        data = model.All();
        dialogueData = standDialogueModel.All();
        SendDataToChild();
    }
    private void SendDataToChild()
    {
        if (data == null) return;
        int totalExpenses = 0;
        
        for (int i = 0; i < data.Count; i++)
        {
            childControllers[i].SetDependencies(data[i], model, globalStatsController, collectAuraPoint);
            int price = (int)(long)data[i]["price"];
            int level = (int)(long)data[i]["level"];
            if((string) data[i]["active_state"]=="unlocked")
                totalExpenses += StatsController.GetExpense(level, price);
        }
        globalExpensesController.UpdateExpenses(totalExpenses);
    }
    private void SetExpenses(int id)
    {
        Dictionary<string, object> entry = model.Get(id)[0];
        int level = (int)(long)entry["level"];
        int price = (int)(long)entry["price"];
        int expenses = globalExpensesController.GetExpenses() + StatsController.GetExpense(level, price);
        globalExpensesController.UpdateExpenses(expenses);
    }
    private void UnregisterEvent()
    {
        EventList.OnPurchasedStand.Unsubscribe(SetExpenses);
        EventList.OnUpgradeStand.Unsubscribe(SetExpenses);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }




}
