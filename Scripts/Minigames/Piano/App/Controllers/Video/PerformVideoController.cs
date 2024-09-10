using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Events;

public class PerformVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Slider videoProgress;
    public int totalTiles;

    private double videoLength, tileInterval;
    private Coroutine updateVideoProgress;
    private void Start()
    {
        videoPlayer.loopPointReached += StopPlay;
    }
    public void SetVideoClip(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoLength = clip.length;
    }
    public double GetTileInterval ()
    {
        tileInterval = videoPlayer.clip.length/totalTiles;
        return tileInterval;
    }
    public void StartPlay()
    {
        updateVideoProgress = StartCoroutine(TimerController.SetPreciseInterval((float) tileInterval, UpdateVideoProgress));
        videoPlayer.Play();
    }
    public void StopPlay(VideoPlayer _)
    {
        
        EventList.OnChangeGameState.Trigger(false);
    }
    public void GameOver()
    {

    }
    private void UpdateVideoProgress()
    {
        videoProgress.value = (float)(videoPlayer.time / videoLength);
    }
    private void BlankVideo()
    {
        videoPlayer.clip = null;
    }
    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= StopPlay;
    }
}
