using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*public class Stats
{
    public int aura, ticket, exp, targetExp, level;
    public Stats(List<Dictionary<string, object>> data)
    {
        SetData(data);
    }
    public void SetData(List<Dictionary<string, object>> data)
    {
        if (data != null || data.Count > 0)
        {
            aura = (int)data[0]["aura"];
            ticket = (int)data[0]["ticket"];
            exp = (int)data[0]["exp"];
            targetExp = (int)data[0]["targetExp"];
            level = (int)data[0]["level"];
        }
    }
}*/
public class StatsView : MonoBehaviour
{
    public List<Dictionary<string, object>> data;
    public TMP_Text auraText, ticketText, expText, levelText;
    public TMP_Text auraUpdateText, ticketUpdateText;
    public Slider expProgress;
    private Coroutine playAuraUpdateText, playTicketUpdateText;
    //public Button createButton;
    
    public void Render(List<Dictionary<string, object>> _data)
    {
        data = _data;
        SetView();
    }
    private void SetView()
    {
        if (data == null) return;
        if (data.Count == 0) return;
        string oldAuraValue = auraText.text;
        string oldTicketValue = ticketText.text;
        auraText.SetText(data[0]["aura"].ToString());
        ticketText.SetText(data[0]["ticket"].ToString());
        
        
        /*expText.SetText($"{data[0]["exp"]}/{data[0]["targetExp"]}");
        levelText.SetText(data[0]["level"].ToString());
        expProgress.value =  (float) (int) data[0]["exp"] / (int) data[0]["targetExp"];*/
    }
    public void ShowTicketUpdateText(string _updateTextContent) {
        ticketUpdateText.SetText(_updateTextContent);
        playTicketUpdateText=StartCoroutine(PlayUpdateTextAnim(ticketUpdateText, playTicketUpdateText));

    }public void ShowAuraUpdateText(string _updateTextContent) {
        auraUpdateText.SetText(_updateTextContent);
        playAuraUpdateText=StartCoroutine(PlayUpdateTextAnim(auraUpdateText, playAuraUpdateText));

    }
    private IEnumerator PlayUpdateTextAnim(TMP_Text _text, Coroutine coroutine)
    {
        _text.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _text.gameObject.SetActive(false);
        if(coroutine!=null) StopCoroutine(coroutine); 
    }
        

   
}
