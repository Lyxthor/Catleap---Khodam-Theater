using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStoryView : MonoBehaviour
{
    public GameObject stage;
    public Image banner;
    public Button nextButton, prevButton, performButton, openButton;
    private Dictionary<string, object> data;
    Color lockedColor=Color.black, enableColor=Color.white, disableColor=Color.red;
    

    private void Start()
    {
        
    }
    public void Render(Dictionary<string, object> _data)
    {
        if (_data == null) return;
        if (_data.Count == 0) return;
        Texture2D texture = Resources.Load<Texture2D>($"{_data["image_url"]}");
        if (texture != null)
        {
            banner.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
            Color color = (string)_data["active_state"] == "unlocked" ? enableColor : Color.black;
            banner.color = color;
        }
    }
    public void EnableView(bool _isEnabled)
    {
        openButton.enabled = _isEnabled;
        if (_isEnabled)
        {
            stage.GetComponent<SpriteRenderer>().color = enableColor;
            openButton.GetComponent<Image>().color = enableColor;
            return;
        }
        stage.GetComponent<SpriteRenderer>().color = disableColor;
        openButton.GetComponent<Image>().color = disableColor;
    }
}
