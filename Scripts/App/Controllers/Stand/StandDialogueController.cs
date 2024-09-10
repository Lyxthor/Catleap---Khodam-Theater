using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StandDialogueController : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    private List<Dictionary<string, object>> dialogueData;
    
    public void SetData(List<Dictionary<string, object>> _dialogueData)
    {
        Debug.Log(_dialogueData.Count);
        if (_dialogueData == null) return;
        if (_dialogueData.Count == 0) return;
        dialogueData = _dialogueData;
    }
    public void DisplayDialogueBox()
    {
        dialogueText.SetText(RandomDialogueText());
        dialogueBox.SetActive(true);
    }
    public void CloseDialogueBox()
    {
        dialogueText.SetText(string.Empty);
        dialogueBox.SetActive(false);
    }
    private string RandomDialogueText()
    {
        int randomDialogueIndex = Random.Range(0, dialogueData.Count);
        return (string)dialogueData[randomDialogueIndex]["dialogue_text"];
    }
}
