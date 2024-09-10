using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameStatsView : MonoBehaviour
{
    public TMP_Text auraText, ticketText;
    private Dictionary<string, object> data;

    public void Render(List<Dictionary<string, object>> _data)
    {
        if (_data == null) return;
        if (_data.Count == 0) return;
        data = _data[0];
        auraText.SetText($"{data["aura"]}");
        ticketText.SetText($"{data["ticket"]}");
    }
}
