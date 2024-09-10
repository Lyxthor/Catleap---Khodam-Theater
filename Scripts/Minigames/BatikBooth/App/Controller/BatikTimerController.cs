using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class BatikTimerController : MonoBehaviour
{
    public int interval;
    private TimerController timerController;
    private void Awake()
    {
        
        CustomizeTimer();
        EventRegister();
        timerController.SetPaused(true);
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(StopTimer);
        EventList.OnChangeGameState.Subscribe(ResumeTimer);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(StopTimer);
        EventList.OnChangeGameState.Unsubscribe(ResumeTimer);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }
    private void ResumeTimer(bool _gameState)
    {
        if (_gameState) timerController.SetPaused(false);
    }
    private void CustomizeTimer()
    {
        timerController = GetComponent<TimerController>();
        timerController.SetData(interval, GameOver);
    }
    private void StopTimer(bool _gameState)
    {
        if(!_gameState) timerController.StopAllCoroutines();
    }
    private void GameOver()
    {
        EventList.OnChangeGameState.Trigger(false);
    }
}
