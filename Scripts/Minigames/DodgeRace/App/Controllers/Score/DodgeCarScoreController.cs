using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Events;

public class DodgeCarScoreController : MonoBehaviour
{
    public GameObject curtain;
    public TMP_Text scoreText, resultText;
    public int minPointTicket;
    private int score=0;
    private StatsModel statsModel;
    

    private void Start()
    {
        statsModel = new StatsModel();
        EventRegister();
        Render();
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(CalculateScoreTicket);
        EventList.OnChangeGameState.Subscribe(SetCurtain);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(CalculateScoreTicket);
        EventList.OnChangeGameState.Unsubscribe(SetCurtain);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }
    public void Render()
    {
        scoreText.SetText($"{score}");
    }
    public void UpdateScore(int updateAmount)
    {
        score += updateAmount;
        Render();
    }
    private void CalculateScoreTicket(bool gameState)
    {
        if (!gameState)
        {
            Debug.Log("test");
            
            List<Dictionary<string, object>> data = statsModel.Get();
            int ticketEarn = Mathf.CeilToInt((float)score / minPointTicket);
            resultText.SetText($"{score}\n{ticketEarn}");
            data[0]["ticket"] = (int)data[0]["ticket"] + ticketEarn;
            statsModel.CreateOrUpdate(data);
            /*resultScoreText.SetText($"{score}");
            ticketEarnText.SetText($"{ticketEarn}");*/
        }
    
    }
    
    private void SetCurtain(bool gameState)
    {
        if (!gameState) StartCoroutine(TimerController.SetTimeout(2, CurtainDown));
    }
    private void CurtainDown()
    {
        StopAllCoroutines();
        CurtainAnimationController curtainController = curtain.GetComponent<CurtainAnimationController>();
        curtainController.MoveDown();
    }
    

}
