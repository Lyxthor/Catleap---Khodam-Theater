using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelView : MonoBehaviour
{
    public GameObject panel, loader; 
    public Button[] openButton, closeButton;
    public Button nextButton;
    public bool withTransitionIn, withTransitionOut, haveSequence;
    public int transitionDuration;
    private GameObject parent;
    private void Start()
    {
        parent = panel.transform.parent.gameObject;
        AssignOpenCloseButton(openButton);
        AssignOpenCloseButton(closeButton);
        if (nextButton != null) nextButton.onClick.AddListener(NextSequence);
    }
    private void OnEnable()
    {
        if(!withTransitionIn && !withTransitionOut)
        {
            Animator anim = panel.GetComponent<Animator>();
            if(anim!=null)
            {
                anim.enabled = false;
            }
        }
    }
    private void NextSequence()
    {
        RunPanelTransition(false);
        StartCoroutine(TimerController.SetTimeout(transitionDuration * 2, delegate { ActivateBoard(true); }));
    }
    private void ActivateBoard(bool state)
    {
        panel.SetActive(state);
        StopAllCoroutines();
    }
    private void TogglePanel()
    {
        bool activeState = !panel.activeSelf;
        if (loader != null && activeState) StartCoroutine(PlayLoadingAnimation(activeState));
        else ToggleTransition(activeState); 
    }
    
    private void RunPanelTransition(bool activeState)
    {
        
        if((activeState && withTransitionIn) || (!activeState && withTransitionOut))
        {
            string trigger = activeState ? "PanelTransitionIn" : "PanelTransitionOut";
            Animator anim = panel.GetComponent<Animator>();
            anim.SetTrigger(trigger);
        }
    }
    private void ToggleTransition(bool activeState)
    {
        if(withTransitionIn && activeState)
        {
            ActivatePanel(activeState);
            StartCoroutine(TimerController.SetTimeout(1, delegate { RunPanelTransition(activeState); }));
            return;
        }
        if(withTransitionOut && !activeState) {
            RunPanelTransition(activeState);
            StartCoroutine(TimerController.SetTimeout(transitionDuration, delegate { ActivatePanel(activeState); }));
            return;
        }
        ActivatePanel(activeState);
        
        
    }
    private void ActivatePanel(bool activeState)
    {
        parent.SetActive(activeState);
        panel.SetActive(activeState);
    }
    private IEnumerator PlayLoadingAnimation(bool activeState)
    {
        loader.SetActive(true);
        yield return new WaitForSeconds(1);
        loader.SetActive(false);
        ToggleTransition(activeState);
        
        
    }
    private void AssignOpenCloseButton(Button[] buttons) 
    {
        foreach (Button button in buttons)
            button.onClick.AddListener(TogglePanel);
    }
    
}
