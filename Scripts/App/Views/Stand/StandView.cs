using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StandView : MonoBehaviour
{
    public SpriteRenderer image;
    public Button collectAuraButton;
    public GameObject smokeEffect;
    public TMP_Text updateText;
    private Dictionary<string, object> data;
    private Color lockedColor = Color.black;
    private Color unlockedColor = Color.white;
    private Coroutine toggleEffect;
    public void Render(Dictionary<string, object> _data)
    {
        if (_data == null) return;
        if (_data.Count == 0) return;
        data = _data;
        SetStandView();
    }
    private void SetStandView()
    {
        Texture2D texture = Resources.Load<Texture2D>((string)data["image_url"]);
        if(texture!=null)
        {
            if(image==null)
            {
                image = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
            Debug.Log(data["image_url"]);
            Debug.Log("Apakah null : " + $"{texture == null}");
            Debug.Log($"Apakah renderernya yg null {image == null}");
            image.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
        }
        if ((string)data["active_state"] == "locked") image.color = lockedColor;
        else if ((string)data["active_state"] == "unlocked")
        {
            image.color = unlockedColor;
            smokeEffect.SetActive(true);
            StartCoroutine(TimerController.SetTimeout(2, DeactivateEffect));
        }
    }
    public void ShowCollectAuraButton()
    {
        collectAuraButton.gameObject.SetActive(true);
    }
    private void DeactivateEffect()
    {
        
        smokeEffect.SetActive(false);
        StopAllCoroutines();
        
    }
    public void ShowUpdateText(string _updateTextContent)
    {
        updateText.SetText(_updateTextContent);
        StartCoroutine(PlayUpdateTextAnim());
        
    }
    private IEnumerator PlayUpdateTextAnim()
    {
        updateText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        updateText.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    
}
