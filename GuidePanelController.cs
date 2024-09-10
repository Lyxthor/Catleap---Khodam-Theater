using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanelController : MonoBehaviour
{
    public Image slideContainer;
    public List<Sprite> slideImages;
    public Button nextButton, prevButton, closeButton;

    private int currentSlideIndex;
    private void Start()
    {
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);
    }
    private void OnEnable()
    {
        currentSlideIndex = 0;
        RenderSlide();
    }
    private void NextSlide()
    {
        if(currentSlideIndex+1>=slideImages.Count)
        {
            closeButton.onClick.Invoke();
            return;
        }
        currentSlideIndex++;
        RenderSlide();
    }
    private void PrevSlide()
    {
        if(currentSlideIndex-1<=0)
        {
            return;
        }
        currentSlideIndex--;
        RenderSlide();
    }
    private void RenderSlide()
    {
        slideContainer.sprite = slideImages[currentSlideIndex];
    }
}
