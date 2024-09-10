using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PropertyDialogueParentController : MonoBehaviour
{
    public int displayDialogueInterval, closeDialogueInterval;
    public PropertyDialogueController[] childControllers;
    private List<Dictionary<string, object>> data;
    private PropertyDialogueModel model;
    private Coroutine toggleDialogue;
    private int randomPropertyIndex;

    private void Start()
    {
        model = new PropertyDialogueModel();
        GetData();
    }
    private void GetData()
    {
        data = model.All();
        if (data == null) return;
        if (data.Count == 0) return;
        SendDataToChildren();
    }
    private void SendDataToChildren()
    {
        for(int i=0;i<childControllers.Length;i++)
        {
            int propertyId=childControllers[i].GetPropertyId();
            string propertyType=childControllers[i].GetPropertyType();
            List<Dictionary<string, object>> dataForChild = data.Where(dt => (int)(long)dt["property_id"] == propertyId && (string)dt["property_type"] == propertyType).ToList();
            childControllers[i].SetDialogueData(dataForChild);
        }
        toggleDialogue = StartCoroutine(TimerController.SetTimeout(displayDialogueInterval, DisplayRandomPropertyDialogue));
    }
    private void DisplayRandomPropertyDialogue()
    {
        randomPropertyIndex = Random.Range(0, childControllers.Length);
        PropertyDialogueController choosenChildController = childControllers[randomPropertyIndex];
        if (choosenChildController.GetActiveState() == false) DisplayRandomPropertyDialogue();
        else
        {
            choosenChildController.ShowDialogueBox();
            toggleDialogue = StartCoroutine(TimerController.SetTimeout(closeDialogueInterval, CloseOpenedPropertyDialogue));
        }
    }
    private void CloseOpenedPropertyDialogue()
    {
        PropertyDialogueController choosenChildController = childControllers[randomPropertyIndex];
        choosenChildController.CloseDialogueBox();
        toggleDialogue = StartCoroutine(TimerController.SetTimeout(displayDialogueInterval, DisplayRandomPropertyDialogue));
    }
}
