using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurtainAnimationController : MonoBehaviour
{
    public Button backToMenuButton;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        backToMenuButton.onClick.AddListener(BackToGamePlay);
    }
    private void BackToGamePlay()
    {
        SceneController.Instance.BackToGamePlay(true);
    }
    public void MoveDown()
    {
        anim.SetTrigger("Down");
    }
    public void MoveUp()
    {
        anim.SetTrigger("Up");
    }
}
