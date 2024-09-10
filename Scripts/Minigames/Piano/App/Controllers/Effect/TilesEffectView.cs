using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TilesEffectView : MonoBehaviour
{
    public GameObject[] hitInfos;

    public void ShowHitInfo(string text, Color color)
    {
        GameObject hitInfo = GetUnemployedHitInfo();
        TMP_Text hitInfoText = hitInfo.GetComponent<TMP_Text>();
        Animator hitInfoAnim = hitInfo.GetComponent<Animator>();
        hitInfoText.color = color;
        hitInfoText.SetText(text);
        hitInfoAnim.SetTrigger("Pop");
        hitInfo.SetActive(true);
    }
    private GameObject GetUnemployedHitInfo()
    {
        foreach(GameObject hitInfo in hitInfos)
        {
            if (!hitInfo.activeInHierarchy) return hitInfo;
        }
        return null;
    }
}
