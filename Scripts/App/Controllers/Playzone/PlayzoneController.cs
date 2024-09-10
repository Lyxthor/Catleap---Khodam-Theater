using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayzoneController : MonoBehaviour
{
    private Dictionary<string, object> data;
    private PlayzoneView view;
    private PropertyDialogueController dialogueController;

    private void Awake()
    {
        GetDependency();
        
    }
    private void OnEnable()
    {
        
    }
    private void GetDependency()
    {
        if (view == null)
        {
            view = GetComponent<PlayzoneView>();
            view.playMiniGameButton.onClick.AddListener(PlayMiniGame);
        }
        if(dialogueController==null)
        {
            dialogueController = GetComponent<PropertyDialogueController>();
        }
    }
    public void SetData(Dictionary<string, object> _data)
    {
        GetDependency();
        data = _data;
        view.Render(data);
        dialogueController.SetPropertyData((int)(long)data["id"], "Playzone", true);
    }
    private void PlayMiniGame()
    {
        if (data == null) return;
        if (data.Count == 0) return;
        SceneController.Instance.PlaySceneTransition("BurnIn");
        StartCoroutine(TimerController.SetTimeout(2, RedirectScene));
    }
    private void RedirectScene()
    {
        SceneController.Instance.LoadByName((string)data["scene_name"]);
        StopAllCoroutines();
    }

}
