using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallShopView : MonoBehaviour
{
    public TMP_Text ballAmountText, ballPriceText;
    public Button purchaseButton;
    private Dictionary<string, object> data;
    public void Render(List<Dictionary<string, object>> _data)
    {
        if (_data == null) return;
        if (_data.Count == 0) return;
        data = _data[0];
        ballAmountText.SetText($"x{data["amount"]}");
        ballPriceText.SetText($"{data["price"]}=");
    }
}
