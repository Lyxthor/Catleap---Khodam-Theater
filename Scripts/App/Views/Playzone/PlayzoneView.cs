using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayzoneView : MonoBehaviour
{
    public SpriteRenderer image;
    public Button playMiniGameButton;
    private Dictionary<string, object> data;
 
    public void Render(Dictionary<string, object> _data)
    {
        if (_data == null) return;
        if (_data.Count == 0) return;
        data = _data;
        SetView();
    }
    private void SetView()
    {
        Texture2D texture = Resources.Load<Texture2D>((string)data["image_url"]);
        image.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
    }
    
    
}
