using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadSceneController : MonoBehaviour
{
    public VideoPlayer vp;

    private void Start()
    {
        vp.loopPointReached += Redirect;
    }
    private void Redirect(VideoPlayer vp)
    {
        SceneController.Instance.LoadByName("MainMenu");
    }
}
