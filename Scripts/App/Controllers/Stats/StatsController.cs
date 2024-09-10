using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    private List<Dictionary<string, object>> data;
    private StatsView view;
    private StatsModel model;
    private string updateAuraText, updateTicketText;
    public static int maxLevel=20;
    private void Awake()
    {
        model = new StatsModel();
        view = GetComponent<StatsView>();
        /*if (!model.IsExists()) model.CreateOrUpdate(InitFirstStat());*/
        GetData();
    }
    
    public void GetData()
    {
        data = model.Get();
        view.Render(data);
    }
    public int GetCurrentAuraAmount()
    {
        return (int) data[0]["aura"];
    }
    public int GetCurrentTicketAmount()
    {
        return (int)data[0]["ticket"];
    }
    public void UpdateData(List<Dictionary<string, object>> _data)
    {
        model.CreateOrUpdate(_data);
        GetData();
    }

    public void UpdateAura(int updateAmount)
    {
        view.ShowAuraUpdateText(updateAmount.ToString());
        Debug.Log($"Update Amount : {updateAmount}");
        int currentAuraAmount = (int) data[0]["aura"];
        currentAuraAmount += updateAmount;
        data[0]["aura"] = currentAuraAmount;
        updateAuraText = updateAmount.ToString();
        UpdateData(data);
    }
    public void UpdateTicket(int updateAmount) 
    {
        view.ShowTicketUpdateText(updateAmount.ToString());
        int currentTicketAmount = (int)data[0]["ticket"];
        currentTicketAmount += updateAmount;
        data[0]["ticket"] = currentTicketAmount;
        updateTicketText = updateAmount.ToString();
        UpdateData(data);
    }

    public void LevelUp(List<Dictionary<string, object>> _data)
    {
        _data[0]["level"] = (int)_data[0]["level"] + 1;
        _data[0]["targetExp"] = (int)_data[0]["targetExp"] * 2;
        _data[0]["exp"] = 0;
        UpdateData(_data);
    }

    private List<Dictionary<string, object>> InitFirstStat()
    {
        List<Dictionary<string, object>> firstData = new List<Dictionary<string, object>>();
        Dictionary<string, object> firstEntry = new Dictionary<string, object>();
        firstEntry.Add("aura", 100000);
        firstEntry.Add("ticket", 10);
        firstEntry.Add("exp", 900);
        firstEntry.Add("targetExp", 1000);
        firstEntry.Add("level", 1);
        firstData.Add(firstEntry);
        return firstData;
    }
    public static float GetIncomeRate(int level)
    {
        return (float) level / maxLevel;
    }
    public static float GetUpgradeRate(int level)
    {
        return (float) level / 2;
    }
    public static float GetExpenseRate(int level)
    {
        return GetIncomeRate(level) / 2;
    }
    public static int GetIncome(int level, int price)
    {
        return (int) (GetIncomeRate(level) * price);
    }
    public static int GetUpgradePrice(int level, int price)
    {
        return (int) (GetUpgradeRate(level) * price);
    }
    public static int GetExpense(int level, int price)
    {
        return (int) (GetExpenseRate(level) * price);
    }
}
