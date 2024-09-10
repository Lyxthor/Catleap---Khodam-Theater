using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleController : MonoBehaviour
{
    public TMP_Text ticketPrizeText;
    private int prize;
    public int Prize { 
        get=>prize;
        set
        {
            ticketPrizeText.SetText($"{value}");
            prize = value;
        }
    }
}
