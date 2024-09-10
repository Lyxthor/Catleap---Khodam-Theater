using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class CollectAuraButtonController : MonoBehaviour
{
    public Vector3 target;
    public Action collectAura;
    private Button button;
    private Vector3 origin;
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>();
        origin = transform.position;
    }
    

    // Update is called once per frame
    private void Update()
    {
        button.animator.enabled = false;
        button.transform.position = Vector3.MoveTowards(transform.position, target, 1);
        if (!IsReached()) return;
        DeactivateCollectButton();
    }
    private void DeactivateCollectButton()
    {
        collectAura();
        button.animator.enabled = true;
        transform.position = origin;
        StopAllCoroutines();
        button.gameObject.SetActive(false);
        this.enabled = false;
    }
    private bool IsReached()
    {
        
        return (Vector3.Distance(transform.position, target) < 0.001);
    }
}
