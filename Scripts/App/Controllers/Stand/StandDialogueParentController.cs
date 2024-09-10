using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandDialogueParentController : MonoBehaviour
{
    public int displayDialogueInterval, closeDialogueInterval;
    private int randomStandIndex;
    public StandDialogueController[] standDialogueControllers;
    private Coroutine dialogueAction;
    private void Start()
    {
        //dialogueAction=StartCoroutine(TimerController.SetTimeout(displayDialogueInterval, DialogueDisplay));
    }
    // ONLY DISPLAY STAND DIALOGUE WHEN THERE IS AT LEAST ONE STAND UNLOCKED
    // IF NOT THEN STOP DIALOGUE COROUTINE
    // ALWAYS CHECK IF THERE IS STAND DIALOGUE PURCHASED BY USING EVENT HANDLER


    private void DialogueDisplay()
    {
        randomStandIndex = GetRandomStandIndex();
        standDialogueControllers[randomStandIndex].DisplayDialogueBox();
        dialogueAction = StartCoroutine(TimerController.SetTimeout(closeDialogueInterval, DialogueClose));
    }
    private void DialogueClose()
    {
        standDialogueControllers[randomStandIndex].CloseDialogueBox();
        dialogueAction = StartCoroutine(TimerController.SetTimeout(displayDialogueInterval, DialogueDisplay));
    }
    private int GetRandomStandIndex()
    {
        return Random.Range(0, standDialogueControllers.Length);
    }
}
