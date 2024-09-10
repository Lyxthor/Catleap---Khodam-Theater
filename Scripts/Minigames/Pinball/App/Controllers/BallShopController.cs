using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShopController : MonoBehaviour
{
    public MiniGameStatsController miniGameStatsController;

    private BallShopView view;
    private BallModel model;
    private List<Dictionary<string, object>> data;
    private void Start()
    {
        model = new BallModel();
        view = GetComponent<BallShopView>();
        view.purchaseButton.onClick.AddListener(PurchaseBall);
        GetData();
    }
    private void GetData()
    {
        data = model.Get();
        view.Render(data);
    }

    private void UpdateData(List<Dictionary<string, object>> _data) 
    {
        model.CreateOrUpdate(_data);
        GetData();
    }
    private void PurchaseBall()
    {
        int currentAuraAmount = miniGameStatsController.GetCurrentAuraAmount();
        int ballPrice = (int)data[0]["price"];
        if(currentAuraAmount>=ballPrice)
        {
            miniGameStatsController.UpdateAura(-ballPrice);
            data[0]["amount"] = GetCurrentBallAmount() + 1;
            UpdateData(data);
        }
            
    }
    
    public int GetCurrentBallAmount()
    {
        return (int)data[0]["amount"];
    }
    public void UpdateBall(int updateAmount) {

        data[0]["amount"] = updateAmount;
        UpdateData(data);
    }

}
