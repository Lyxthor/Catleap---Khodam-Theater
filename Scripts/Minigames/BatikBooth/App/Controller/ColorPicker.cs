using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*[System.Serializable]
public class Picker
{
    public string key;
    public Color color;
    public Button picker;
}*/
public class Timer
{
    public static IEnumerator Countdown(decimal waitTime, Action OnEnd, Action OnInterlude)
    {
        while(waitTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            waitTime = Decimal.Subtract(waitTime, (decimal) 0.1);
            OnInterlude?.Invoke();
        }
        OnEnd?.Invoke();
    }
}
public class ColorPicker : MonoBehaviour
{
    private class KeyAnswer
    {
        public string key;
        public string answer;
        public bool status;
        public Button part;
        public KeyAnswer(string _key, string _answer, Button _part)
        {
            key = _key;
            answer = _answer;
            part = _part;
            status = false;
        }
    }
    public GameObject[] canvasImage;
    public GameObject resultPanel;
    public TMP_Text timerDisplay;
    public TMP_Text infoDisplay;
    public TMP_Text scoreDisplay;
    public ColorPick[] colorPickers;
    public float waitTimeField;
    private decimal waitTime;
    private decimal elapsedTime;

    private GameObject answerImage, randomCanvasImage;
    private List<KeyAnswer> keyAnswers;
    private Dictionary<string, Color> keyColorPair;
    private List<string> keys;
    private string pickedColorKey;
    private bool gameStatus;

    private void Start()
    {
        
        keyColorPair = new Dictionary<string, Color>();
        keyAnswers = new List<KeyAnswer>();
        keys = new List<string>();
        GameStart();
        SetKeyColorPair();
        SetAnswerImage();
    }
    public void GameStart()
    {
        resultPanel.SetActive(false);
        gameStatus = true;
        waitTime = (decimal) waitTimeField;
        StartCoroutine(Timer.Countdown(waitTime, GameOver, SetTimerDisplay));
        foreach(KeyAnswer ky in keyAnswers)
        {
            ky.status = false;
            ky.key = string.Empty;
        }
    }
    public void GameOver()
    {
        infoDisplay.SetText($"Total time\t:{elapsedTime}\nRight color : {CalculateRightAmount()}/{keyAnswers.Count}");
        gameStatus = false;
        StopAllCoroutines();
        resultPanel.SetActive(true);
    }
    public void SetTimerDisplay()
    {
        waitTime = Decimal.Subtract(waitTime, (decimal)0.1);
        elapsedTime = Decimal.Add(elapsedTime, (decimal)0.1);
        timerDisplay.SetText($"{waitTime}s");
        if(waitTime<=0)
        {
            GameOver();
        }
    }
    private void SetKeyColorPair()
    {
        foreach (ColorPick colorPicker in colorPickers)
        {
            keyColorPair.Add(colorPicker.key, colorPicker.color);
            keys.Add(colorPicker.key);
            colorPicker.picker.onClick.AddListener(delegate { PickColor(colorPicker.key); });
            colorPicker.picker.GetComponent<Image>().color = keyColorPair[colorPicker.key];
        }
    }
    private void PickColor(string colorKey)
    {
        if(gameStatus) pickedColorKey = colorKey;
    }
    private void DropColor(KeyAnswer _keyAnswer)
    {
        if(pickedColorKey!=null&&gameStatus)
        {
            _keyAnswer.key = pickedColorKey;
            _keyAnswer.part.GetComponent<Image>().color = keyColorPair[pickedColorKey];
            SetAnswerStatus(_keyAnswer);
        }
    }
    private void SetAnswerImage()
    {
        InitAnswerImage();
        List<Button> answerParts = GetAllChildren(answerImage.transform);
        List<Button> canvasParts = GetAllChildren(randomCanvasImage.transform);
        for(int i=0;i<canvasParts.Count;i++)
        {
            string colorKey = GetRandColorKey();
            SetImageColor(answerParts[i].GetComponent<Image>(), keyColorPair[colorKey]);
            KeyAnswer newKeyAnswer = new KeyAnswer(null, colorKey, canvasParts[i]);
            keyAnswers.Add(newKeyAnswer);
            canvasParts[i].onClick.AddListener(delegate { DropColor(newKeyAnswer); });
        }
    }
    private void InitAnswerImage()
    {
        randomCanvasImage = canvasImage[UnityEngine.Random.Range(0, 1)];
        answerImage = Instantiate<GameObject>(randomCanvasImage);
        answerImage.transform.localScale = new Vector3(0.25f, 0.25f, 0);
        answerImage.transform.SetParent(transform, false);
        answerImage.transform.position = randomCanvasImage.transform.position;
    }
    private List<Button> GetAllChildren(Transform parent)
    {
        List<Button> children=new List<Button>();
        foreach(Transform child in parent)
            children.Add(child.GetComponent<Button>());
        return children;
    }
    private void SetAnswerStatus(KeyAnswer _keyAnswer)
    {
        _keyAnswer.status = _keyAnswer.key == _keyAnswer.answer;
        if (CalculateRightAmount() == keyAnswers.Count) GameOver();
    }
    private int CalculateRightAmount()
    {
        int amount = 0;
        foreach (KeyAnswer ka in keyAnswers)
            if (ka.status) amount++;
        return amount;
    }
    private string GetRandColorKey()
    {
        int randIndex = UnityEngine.Random.Range(0,colorPickers.Length-1);
        return keys[randIndex];
    }
    private void SetImageColor(Image image, Color color)
    {
        image.color = color;
    }
   
   
    /*[SerializeField]
    private Button[] colorPickers;
    [SerializeField]
    private GameObject partContainer;
    private GameObject answerImage;
    private Button[] parts;

    private Color color;
    private Dictionary<string, Color> palettes;
    private void Start()
    {
        palettes = new Dictionary<string, Color>();
        SetPicker();
        
    }
    private void SetPicker()
    {
        SetPickerAndColor(colorPickers[0],"red", "#FF0000");
        SetPickerAndColor(colorPickers[1], "yellow", "#FFEC00");
        SetPickerAndColor(colorPickers[2], "green", "#33FF00");
        SetPickerAndColor(colorPickers[3], "blue", "#0073FF");
        SetPickerAndColor(colorPickers[4], "purple", "#B600FF");
        SetPickerAndColor(colorPickers[5], "white", "#FFFFFF");
        SetPickerAndColor(colorPickers[6], "pink", "#FF41AF");
        SetPickerAndColor(colorPickers[7], "brown", "#8E4C28");
        SetPickerAndColor(colorPickers[8], "orange", "#FF8500");
        SetPickerAndColor(colorPickers[9], "black", "#000000");
    }
    private void SetPickerAndColor(Button picker,string key,string hex)
    {
        Image pickerImage = picker.GetComponent<Image>();
        palettes.Add(key, ConvertHex(hex));
        pickerImage.color = palettes[key];
        picker.onClick.AddListener(delegate { SetPickerColor(pickerImage); });
    }
    private Color ConvertHex(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
    private void SetPickerColor(Image image) => color = image.color;
    private void SetPartColor(Image image) => image.color = color;
    private void CopyAndSetAnswer()
    {
        answerImage = Instantiate<GameObject>(partContainer);
    }
    private Color GetRandColor()
    {
        string[] key = { "red", "yellow", "green", "blue", "purple", "white", "pink", "brown", "orange", "purple" };
        int randomKey=Random.Range(0, 9);
        return palettes[key[randomKey]];
    }*/

}
