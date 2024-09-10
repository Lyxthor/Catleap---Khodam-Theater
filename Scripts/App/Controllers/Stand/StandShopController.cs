
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class StandShopController : MonoBehaviour
{
    public StatsController globalStatsController;
    private StandShopView view;
    private StandModel model; // THIS MODEL USES SQLITEMODEL, SO BE AWARE OF THE DATA TYPE
    private List<Dictionary<string, object>> data;
    private int entryIndex=0;
    private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    private void Awake()
    {
        GetAllInstances();
    }
    private void OnEnable()
    {
        GetAllInstances();
    }
    private void GetAllInstances()
    {
        if (view == null)
        {
            view = GetComponent<StandShopView>();
            AssignBulkButtonsAction(view.openButton, GetData);
            view.purchaseButton.onClick.AddListener(PurchaseUpgradeStand);
            view.nextButton.onClick.AddListener(PrevEntry);
            view.prevButton.onClick.AddListener(NextEntry);
        }
        if (data == null)
        {
            model = new StandModel();
            data = new List<Dictionary<string, object>>();
            GetData();
        }
    }
    // CRUD START
    private void GetData()
    {
        data = model.All();
        if (data == null) return;
        if (data.Count == 0) return;
        view.Render(data[entryIndex]);
    }
    private void UpdateEntry(Dictionary<string, object> _entry)
    {
        model.Update(_entry, (int)(long) _entry["id"]);
        view.Render(data[entryIndex]);
    }
    // CRUD END

    // NAVIGATE DATA START
    private void PrevEntry()
    {
        entryIndex = entryIndex > 0 ? entryIndex - 1 : data.Count-1;
        view.Render(data[entryIndex]);
    }
    private void NextEntry()
    {
        entryIndex = entryIndex < data.Count-1 ? entryIndex + 1 : 0;
        view.Render(data[entryIndex]);
    }
    // NAVIGATE DATA END

    // GAME MECHANICS START
    private void PurchaseUpgradeStand()
    {
        data = model.All();
        string activeState=(string)data[entryIndex]["active_state"];
        if (activeState == "locked") PurchaseStand();
        else if (activeState == "unlocked") UpgradeStand();
    }
    private void PurchaseStand()
    {

        int price = (int)(long)data[entryIndex]["price"];
        if (NotEnoughAura(price))
        {
            
            return;
        }
        globalStatsController.UpdateAura(-price);
        data[entryIndex]["active_state"] = "unlocked";
        data[entryIndex]["last_get_income_time"] = DateTime.Now.ToString(dateTimeFormat);
        UpdateEntry(data[entryIndex]);
        
        EventList.OnPurchasedStand.Trigger((int)(long)data[entryIndex]["id"]);
    }
    private void UpgradeStand()
    {
        int price = (int)(long)data[entryIndex]["price"];
        int level = (int)(long)data[entryIndex]["level"];
        int upgradePrice = StatsController.GetUpgradePrice(price, level);
        if (NotEnoughAura(upgradePrice))
        {
           
            return;
        }
        globalStatsController.UpdateAura(-upgradePrice);
        data[entryIndex]["level"] = (long)level + 1;
        UpdateEntry(data[entryIndex]);
        
        EventList.OnUpgradeStand.Trigger((int)(long)data[entryIndex]["id"]);
    }
    private bool NotEnoughAura(int price)
    {
        int currentAuraAmount = globalStatsController.GetCurrentAuraAmount();
        return price > currentAuraAmount;
    }
    // GAME MECHANICS END

    // HELPERS START
    private void AssignBulkButtonsAction(Button[] buttons, Action action)
    {
        foreach(Button button in buttons)
            button.onClick.AddListener(delegate { action(); });
    }
    // HELPERS END
}
