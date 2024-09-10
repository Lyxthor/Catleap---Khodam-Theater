using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerView : MonoBehaviour
{
    public TMP_Text timerText;
    public void SetTimerText(int interval)
    {
        timerText.SetText($"{interval}");
    }
}
