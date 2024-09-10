using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using TMPro;

public class GameStateController : MonoBehaviour
{
    public TimerController timerController;
    public TMP_Text gameStateText;
    private void Awake()
    {
        EventRegister();
        timerController.SetData(3, GameStart);
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(GameOver);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(GameOver);
        
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }
    private void GameStart()
    {
        gameStateText.gameObject.SetActive(false);
        Destroy(timerController.gameObject);
        EventList.OnChangeGameState.Trigger(true);
    }
    private void GameOver(bool gameState)
    {
        if (!gameState)
        {
            gameStateText.SetText("Game Over");
            gameStateText.gameObject.SetActive(true);
        }
    }
}
