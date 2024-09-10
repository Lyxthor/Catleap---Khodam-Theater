using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyDialogueController : MonoBehaviour
{
    public SpriteRenderer dialogueImage;
    private Dictionary<string, object> propertyData;
    private List<Dictionary<string, object>>  dialogueData;
    private int propertyId;
    private string propertyType;
    private bool activeState;
    public void SetPropertyData(int _propertyId, string _propertyType, bool _activeState)
    {
        propertyId = _propertyId;
        propertyType = _propertyType;
        activeState= _activeState;

    }
    public void SetDialogueData(List<Dictionary<string, object>> _dialogueData)
    {
        dialogueData = _dialogueData;
    }
    public int GetPropertyId()
    {
        return propertyId;
    }
    public string GetPropertyType()
    {
        return propertyType;
    }
    public bool GetActiveState()
    {
        return activeState;
    }
    public void ShowDialogueBox()
    {
        int randomDialogueIndex = Random.Range(0, dialogueData.Count-1);
        Texture2D texture = Resources.Load<Texture2D>($"{dialogueData[randomDialogueIndex]["dialogue_box_url"]}");
        if (texture != null)
            dialogueImage.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
        dialogueImage.gameObject.SetActive(true);
    }
    public void CloseDialogueBox()
    {
        dialogueImage.gameObject.SetActive(false);
    }
}
