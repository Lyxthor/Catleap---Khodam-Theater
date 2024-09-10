using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Events;

public class DodgeCarGameStateController : MonoBehaviour
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
    private void GameStart()
    {
        gameStateText.gameObject.SetActive(false);
        Destroy(timerController.gameObject);
        EventList.OnChangeGameState.Trigger(true);
    }
    private void GameOver(bool gameState)
    {
        if(!gameState)
        {
            gameStateText.SetText("Game Over");
            gameStateText.gameObject.SetActive(true);
        }
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(GameOver);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }

}
