using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaView : MonoBehaviour
{
    public Button pull1, pull10, nextButton, skipButton, returnButton, closeResultButton;
    public GameObject gachaPage, gachaResult;
    public GachaController controller;
    public GachaVideoController videoController;
    public List<GameObject> prizeHolders;
    public StatsController statsController;
    private Coroutine changeGachaState;
    private Animator gachaAnim, resultAnim;
    private List<Prize> prizeList;
    private int sequenceIndex;

    private void Start()
    {
        pull1.onClick.AddListener(delegate { StartGacha(1); });
        pull10.onClick.AddListener(delegate { StartGacha(10); });
        nextButton.onClick.AddListener(LoadNextSequence);
        skipButton.onClick.AddListener(SkipAnimationSequences);
        returnButton.onClick.AddListener(BackToGameplay);
        closeResultButton.onClick.AddListener(CloseResult);
        gachaAnim = gachaPage.GetComponent<Animator>();
        resultAnim = gachaResult.GetComponent<Animator>();
    }
    private void BackToGameplay()
    {
        SceneController.Instance.BackToGamePlay(true);
    }
    private void StartGacha(int rollAmount)
    {
        int currentTicketAmount = statsController.GetCurrentTicketAmount();
        if (rollAmount > currentTicketAmount) return;
        RollGacha(rollAmount);
        statsController.UpdateTicket(-rollAmount);
        gachaAnim.SetTrigger("GachaOut");
        changeGachaState = StartCoroutine(TimerController.SetTimeout(3, StartAnimationSequences));
    }
    private void EndGacha()
    {
        gachaResult.SetActive(false);
        gachaAnim.SetTrigger("GachaIn");
        CalculateAuraPrize();
        ResetGacha();
    }
    private void CloseResult()
    {
        resultAnim.SetTrigger("ResultOut");
        changeGachaState = StartCoroutine(TimerController.SetTimeout(2, EndGacha));
    }
    private void CalculateAuraPrize()
    {
        int totalAura = 0;
        for(int i=0;i<prizeList.Count;i++)
        {
            prizeList[i].Gift();
            totalAura += prizeList[i].GiveAway();
            prizeHolders[i].SetActive(false);
        }
        statsController.UpdateAura(totalAura);
        Debug.Log(totalAura);
    }
    private void ResetGacha()
    {
        sequenceIndex = 0;
        prizeList = new List<Prize>();
        controller.UpdatePrizeData();
    }
    private void RollGacha(int rollAmount)
    {
        prizeList = controller.Roll(rollAmount);
    }
    private void StartAnimationSequences()
    {
        LoadNextSequence();
        /*int sequenceIndex = 0;
        foreach(Prize prize in prizeList)
        {
            string videoPath = prizeList[sequenceIndex].VideoPath;
            string imagePath = prizeList[sequenceIndex].ImagePath;
            string titleText = prizeList[sequenceIndex].TitleText;
            videoController.PlayGachaAnimation(videoPath, imagePath, titleText);
            sequenceIndex++;
        }*/
    }
    private void LoadNextSequence()
    {
        if (prizeList == null) return;
        if (prizeList.Count == 0) return;
        if (sequenceIndex == prizeList.Count) ShowResult();
        else
        {
            string videoPath = prizeList[sequenceIndex].VideoPath.ToString();
            string imagePath = prizeList[sequenceIndex].ImagePath;
            string titleText = prizeList[sequenceIndex].TitleText;
            bool useImage = prizeList[sequenceIndex].UseImage();
            
            videoController.PlayGachaAnimation(videoPath, imagePath, titleText, useImage);
            sequenceIndex++;
        }
    }
    private void ShowResult()
    {
        videoController.StopVideo();
        gachaResult.SetActive(true);
        AssignPrizeHolders();
        resultAnim.SetTrigger("ResultIn");
    }
    private void AssignPrizeHolders()
    {
        for(int i=0;i<prizeList.Count;i++)
        {
            prizeHolders[i].SetActive(true);
            Image image = prizeHolders[i].transform.GetChild(0).GetComponent<Image>();
            TMP_Text text = prizeHolders[i].transform.GetChild(1).GetComponent<TMP_Text>();

            Texture2D texture = Resources.Load<Texture2D>(prizeList[i].ImagePath);
            Rect imageSize = new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height));
            Vector2 imagePivot = new Vector2(0.5f, 0.5f);
            image.sprite = Sprite.Create(texture, imageSize, imagePivot);

            text.SetText($"{prizeList[i].TitleText}");
        }
    }
    private void SkipAnimationSequences()
    {
        sequenceIndex = prizeList.Count;
        LoadNextSequence();
    }
    private void EndAnimationSequences()
    {
        Debug.Log("Animation Sequences : END");
    }
}
