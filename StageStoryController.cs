using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStoryController : MonoBehaviour
{
    public StageTimer timer;
    private StageStoryView view;
    private StoryModel model;
    private List<Dictionary<string, object>> data;
    private int entryIndex;
    
    private void Start()
    {
        view = GetComponent<StageStoryView>();
        model = new StoryModel();
        GetData();
        view.prevButton.onClick.AddListener(PrevEntry);
        view.nextButton.onClick.AddListener(NextEntry);
        view.performButton.onClick.AddListener(Perform);
    }
    private void OnEnable()
    {
        if(data==null)
        {
            view = GetComponent<StageStoryView>();
            model = new StoryModel();
            GetData();
            view.prevButton.onClick.AddListener(PrevEntry);
            view.nextButton.onClick.AddListener(NextEntry);
            view.performButton.onClick.AddListener(Perform);
        }
    }
    private void GetData()
    {
        data = model.All();
        if (data == null) return;
        if (data.Count == 0) return;
        view.Render(data[entryIndex]);
    }
    private void PrevEntry()
    {
        entryIndex = entryIndex > 0 ? entryIndex - 1 : data.Count - 1;
        view.Render(data[entryIndex]);
    }
    private void NextEntry()
    {
        entryIndex = entryIndex < data.Count - 1 ? entryIndex + 1 : 0;
        view.Render(data[entryIndex]);
    }
    private void Perform()
    {
        Dictionary<string, object> entry = data[entryIndex];
        if((string) entry["active_state"] == "unlocked"&&StageInfoCourier.Instance.isStageEnabled)
        {
            timer.SetCurrentTime();
            StageInfoCourier.Instance.isStageEnabled = false;
            StageInfoCourier.Instance.stageData = entry;
            SceneController.Instance.LoadByName("RythmGame", true);
        }
    }
    
}
