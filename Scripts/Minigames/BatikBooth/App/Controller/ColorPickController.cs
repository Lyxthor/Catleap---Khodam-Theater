using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Events;

[Serializable]
public class ColorPick
{
    public string key;
    public Color color;
    public Button picker;
}
public class ColorPickController : MonoBehaviour
{
    public BatikScoreManager scoreController;
    public Button toggleAnswerVisibilityButton, resetColorButton;
    public List<GameObject> paintings;
    public List<ColorPick> colorPickers;

    private Color baseColor;
    private GameObject selectedPainting, answerPainting;
    private Dictionary<string, Color> keyColorPair;
    private Dictionary<int, bool> answers;
    private string pickedColorKey;
    private void Awake()
    {
        EventRegister();
    }
    private void Start()
    {
        answers = new Dictionary<int, bool>();
        keyColorPair = new Dictionary<string, Color>();
        toggleAnswerVisibilityButton.onClick.AddListener(ToggleAnswerVisibility);
        resetColorButton.onClick.AddListener(ResetColor);
        SetPickerImageColor();
        SelectRandomPainting();
        ToggleAnswerVisibility();
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(ActivateAnswer);
    }
    private void EventUnregister()
    {
        EventList.OnChangeGameState.Unsubscribe(ActivateAnswer);
    }
    private void OnDestroy()
    {
        EventUnregister();
    }
    private void ActivateAnswer(bool _gameState)
    {
        if(_gameState) ToggleAnswerVisibility();
    }
    private void SetPickerImageColor()
    {
        foreach(ColorPick pick in colorPickers)
        {
            keyColorPair.Add(pick.key, pick.color);
            pick.picker.GetComponent<Image>().color = pick.color;
            pick.picker.onClick.AddListener(delegate { PickColor(pick.key); });
        }
    }
    private void SelectRandomPainting()
    {
        selectedPainting = paintings[UnityEngine.Random.Range(0, 2)];
        CreateAnswer();
    }
    private void PickColor(string _key)
    {
        pickedColorKey = _key;
    }
    private void CreateAnswer()
    {
        int childCount = selectedPainting.transform.childCount;
        answerPainting = Instantiate(selectedPainting);
        answerPainting.transform.SetParent(selectedPainting.transform.parent, false);
        int i; 
        for (i=0; i < childCount;i++) 
        {
            string randomKey = colorPickers[UnityEngine.Random.Range(0, colorPickers.Count-1)].key;
            Color randomColor = keyColorPair[randomKey];
            Button partButton = selectedPainting.transform.GetChild(i).GetComponent<Button>();
            Button answerButton = answerPainting.transform.GetChild(i).GetComponent<Button>();
            int answerIndex = i;
            partButton.onClick.AddListener(delegate { DropColor(randomKey, partButton, answerIndex); });
            answerButton.GetComponent<Image>().color = randomColor;
            answers.Add(i, false);
        }
        scoreController.SetMaxScore(i);
        selectedPainting.SetActive(true);
        answerPainting.SetActive(true);
    }
    
    private void DropColor(string _key, Button _partButton, int _answerIndex)
    {
        bool answer = _key == pickedColorKey;
        if (!answers.ContainsKey(_answerIndex)) answers.Add(_answerIndex, answer);
        else answers[_answerIndex] = answer;
        _partButton.GetComponent<Image>().color = keyColorPair[pickedColorKey];
        int updateScore = answer ? 1 : -1;
        scoreController.UpdateScore(updateScore);
        CheckIsEveryAnswersRight();
    }
    private void ToggleAnswerVisibility()
    {
        answerPainting.SetActive(!answerPainting.activeSelf);
    }
    private void ResetColor()
    {
        foreach(Transform child in selectedPainting.transform)
        {
            child.GetComponent<Image>().color = Color.white;
            answers = new Dictionary<int, bool>();
        }
    }
    private void CheckIsEveryAnswersRight()
    {
        bool result = true;
        foreach(KeyValuePair<int, bool> answer in answers)
        {
            if (!answer.Value)
            {
                result = false;
                break;
            }
            
        }
        if (result) EventList.OnChangeGameState.Trigger(false);
    }
}
