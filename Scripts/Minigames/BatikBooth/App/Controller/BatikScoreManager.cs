using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Events;

public class BatikScoreManager : MonoBehaviour
{
    public CurtainAnimationController curtainController;
    public TimerController timerController;
    public TMP_Text resultText;
    public int maxTicketPrize=10; // mis:10
    public int minColoringTime=5;

    private StatsModel statsModel;
    private int score, maxScore;
    private int currentTime, maxTime;

    private void Start()
    {
        statsModel = new StatsModel();
        EventRegister();
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(CalculateScoreResult);
        EventList.OnChangeGameState.Subscribe(SetCurtain);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(CalculateScoreResult);
        EventList.OnChangeGameState.Unsubscribe(SetCurtain);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }
    public void UpdateScore(int _score)
    {
        if(_score > 0 && score>=0)
        {
            score += _score;
        }
    }
    public void SetMaxScore(int _maxScore) { 
        maxScore = _maxScore;
    } 
    private void CalculateScoreResult(bool _gameState)
    {
        if(!_gameState)
        {
            List<Dictionary<string, object>> data = statsModel.Get();
            
            currentTime = timerController.GetCurrent();
            maxTime = timerController.GetInterval();
            currentTime = currentTime < 0 ? 1 : currentTime*2;
            float multiplier = (float)score / maxScore;
            float timePerTicket = ((float)maxTime - minColoringTime) / maxTicketPrize;
            int ticketEarned = Mathf.RoundToInt(currentTime/(timePerTicket)*multiplier);
            Debug.Log(multiplier);
            ticketEarned = ticketEarned > maxTicketPrize ? maxTicketPrize : ticketEarned;
            resultText.SetText($"{score}\n{ticketEarned}");
            data[0]["ticket"] = (int)data[0]["ticket"] + ticketEarned;
            statsModel.CreateOrUpdate(data);
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
