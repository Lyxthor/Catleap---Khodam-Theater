using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button optionButton, closeButton;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        optionButton.onClick.AddListener(OpenOption);
        closeButton.onClick.AddListener(CloseOption);
    }
    private void OpenOption()
    {
        anim.SetTrigger("FadeOut");
    }
    private void CloseOption()
    {
        anim.SetTrigger("FadeIn");
    }
}
