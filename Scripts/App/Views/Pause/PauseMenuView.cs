using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    public Button resumeButton, quitButton;
    public StandParentController standParentController;
    private void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
    }
    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
