using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedirectPage : MonoBehaviour
{
    public Button[] redirectButtons;
    public string sceneDestination;

    private void Start()
    {
        foreach(Button rb in redirectButtons)
        {
            rb.onClick.AddListener(Redirect);
        }
    }
    private void Redirect()
    {
        SceneController.Instance.LoadByName(sceneDestination, true);
    }
}
