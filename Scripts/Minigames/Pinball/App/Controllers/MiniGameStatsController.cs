using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniGameStatsController : MonoBehaviour
{
    private StatsModel model;
    private MiniGameStatsView view;
    private List<Dictionary<string, object>> data;

    private void Start()
    {
        model = new StatsModel();
        view = GetComponent<MiniGameStatsView>();
        GetData();
    }
    private void GetData()
    {
        data = model.Get();
        view.Render(data);
    }
    public int GetCurrentAuraAmount()
    {
        return (int)data[0]["aura"];
    }
    public void UpdateData(List<Dictionary<string, object>> _data)
    {
        model.CreateOrUpdate(_data);
        GetData();
    }
    public void UpdateAura(int updateAmount)
    {
        Debug.Log($"Update Amount : {updateAmount}");
        int currentAuraAmount = (int)data[0]["aura"];
        currentAuraAmount += updateAmount;
        data[0]["aura"] = currentAuraAmount;
        UpdateData(data);
    }
    public void UpdateTicket(int updateAmount)
    {
        Debug.Log($"Update Amount : {updateAmount}");
        int currentTicketAmount = (int)data[0]["ticket"];
        currentTicketAmount += updateAmount;
        data[0]["ticket"] = currentTicketAmount;
        UpdateData(data);
    }
}
