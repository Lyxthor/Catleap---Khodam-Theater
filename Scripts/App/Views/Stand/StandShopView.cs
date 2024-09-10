using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StandShopView : MonoBehaviour
{
    public TMP_Text nameText, descText, priceText, statsInfoText, statusText;
    public Button[] openButton, closeButton;
    public Button purchaseButton, nextButton, prevButton;
    public Image image;
    private Dictionary<string, object> entry;
    private int level, price;
    private void Start()
    {
        openButton[0].onClick.AddListener(delegate { Debug.Log("Pencet loro tenan to"); });
    }
    public void Render(Dictionary<string, object> _entry)
    {
        if (_entry == null) return;
        if (_entry.Count == 0) return;
        SetData(_entry);
        SetContent();
    }
    private void SetData(Dictionary<string, object> _entry)
    {
        entry = _entry;
        level = (int)(long)entry["level"];
        price = (int)(long)entry["price"];
    }
    private void SetContent()
    {
        nameText.SetText(entry["name"].ToString());
        descText.SetText(entry["desc"].ToString());
        SetThumbImg();
        SetPurchaseUpgradeButton();
        SetStatsInfo();
    }
    private void SetThumbImg()
    {
        Texture2D texture = Resources.Load<Texture2D>($"{entry["image_url"]}");
        if (texture != null)
            image.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
    }
    private void SetPurchaseUpgradeButton()
    {
        if (entry["active_state"].ToString() == "unlocked")
        {
            statusText.SetText("UPGRADE");
            priceText.SetText($"{StatsController.GetUpgradePrice(level, price)}");
        }
        else
        {
            statusText.SetText("PURCHASE");
            priceText.SetText($"{entry["price"]}");
        }
            
    }
    private void SetStatsInfo()
    {
        string content = "";
        string levelInfo = $"Lv. {level}\n";
        string incomeInfo = $"Income {StatsController.GetIncome(level, price)}\n";
        string intervalInfo = $"Interval {entry["income_interval"]}\n";
        content += levelInfo + incomeInfo + intervalInfo;
        statsInfoText.SetText(content);
    }
}
