using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class GachaVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoContainer;
    public SpriteRenderer image;
    public TMP_Text title;
    private Coroutine changeColor;
    private Coroutine popUpFading;
    public event Action onVideoEnd;
    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        
    }
    public void WaitVideoToEnd(Action action)
    {
        onVideoEnd += action;
    }
    public void OnVideoEnd(VideoPlayer vp)
    {
        onVideoEnd?.Invoke();
    }
    public void UnWaitVideoToEnd(Action action)
    {
        onVideoEnd -= action;
    }
    public void InitTitleImageTransition()
    {
        WaitVideoToEnd(VanishTitleImage);
    }
    public void PlayGachaAnimation(string _videoPath, string _imagePath, string _titleText, bool _useImage)
    {
        StopAllCoroutines();
        DeactivateTitleImage();
        SetVideo(_videoPath);
        SetImage(_imagePath);
        SetTitle(_titleText);
        PlayAnimation(7, _useImage);
    }
    private void PlayAnimation(int popUpWaitTime, bool useImage) {
        PlayVideo();
        if(useImage)
        {
            ActivateTitleImage();
            popUpFading = StartCoroutine(TimerController.SetTimeout(popUpWaitTime, StartFadingUp));
        }
    }
    public void SetUp(string _videoPath)
    {
        SetVideo(_videoPath);
        PlayVideo();
    }
    public void StopVideo()
    {
        videoPlayer.Stop();
        videoContainer.SetActive(false);
    }
    private void PlayVideo()
    {
        videoContainer.SetActive(true);
        videoPlayer.Play();
    }
    private void StartFadingUp()
    {
        Debug.Log("Fading UP");
        changeColor = StartCoroutine(Fading(Color.white,0));
    }
    private void StartFadingDown()
    {
        Debug.Log("Fading Down");
        changeColor = StartCoroutine(Fading(Color.white,1));
    }
    private void SetVideo(string _videoPath)
    {
        if (videoPlayer.isPlaying) videoPlayer.Stop();
        VideoClip videoClip = Resources.Load<VideoClip>(_videoPath);
        videoPlayer.clip = videoClip;
    }
    private void SetImage(string _imagePath)
    {
        Texture2D imageTexture = Resources.Load<Texture2D>(_imagePath);
        Rect imageSize = new Rect(new Vector2(0, 0), new Vector2(imageTexture.width, imageTexture.height));
        Vector2 imagePivot = new Vector2(0.5f, 0.5f);
        image.sprite = Sprite.Create(imageTexture, imageSize, imagePivot);
    }
    private void SetTitle(string _titleText)
    {
        title.SetText(_titleText);
    }
    private IEnumerator Fading(Color color, int startingAlpha)
    {
        color.a = startingAlpha;
        float alphaChange = startingAlpha == 1 ? -0.01f : 0.01f;
        
        while(NotFaded(color, startingAlpha))
        {
            color.a += alphaChange;
            ChangeTitleColor(color);
            ChangeImageColor(color);
            yield return new WaitForSeconds(0.01f);
        }
        StopCoroutine(changeColor);
    }
    private bool NotFaded(Color color, int startingAlpha)
    {
        return startingAlpha == 1 ? color.a > 0 : color.a < 1;
    }
    
    private void ActivateTitleImage()
    {
        title.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
    }
    private void DeactivateTitleImage()
    {
        Color color = Color.white;
        color.a = 0;
        title.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        title.color = color;
        image.color = color;
        StopAllCoroutines();
    }
    private void VanishTitleImage()
    {
        StartFadingDown();
        popUpFading = StartCoroutine(TimerController.SetTimeout(1, DeactivateTitleImage));
        PlayAnimation(7, true);
    }
    private void ChangeTitleColor(Color color)
    {
        title.color = color;
    }
    private void ChangeImageColor(Color color)
    {
        image.color = color;
    }
    
}
