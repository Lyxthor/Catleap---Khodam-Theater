using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Events;

public class BalloonScoreController : MonoBehaviour
{
    public TMP_Text scoreText, resultText;
    public CurtainAnimationController curtainController;
    public StatsModel statsModel;
    public int minPointTicket;
    private int score=5;

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
    private void Render()
    {
        scoreText.SetText($"{score}");
    }
    public void AddScore()
    {
        score += 2;
        Render();
        CheckIfZero();
    }
    public void MinScore()
    {
        score -= 1;
        Render();
        CheckIfZero();
    }
    private void CheckIfZero()
    {
        if (score == 0) EventList.OnChangeGameState.Trigger(false);
    }
    private void CalculateScoreTicket(bool gameState)
    {
        if (!gameState)
        {
            Debug.Log("test");
            
            List<Dictionary<string, object>> data = statsModel.Get();
            int ticketEarn =  Mathf.FloorToInt((float)score / minPointTicket);
            resultText.SetText($"{score}\n{ticketEarn}");
            data[0]["ticket"] = (int)data[0]["ticket"] + ticketEarn ;
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
        curtainController.MoveDown();
    }
}
