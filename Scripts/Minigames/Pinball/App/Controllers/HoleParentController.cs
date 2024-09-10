using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleParentController : MonoBehaviour
{
    public HoleController[] holes;
    public List<int> ticketPrizes;
    private void Start()
    {
        ticketPrizes = Shuffle(ticketPrizes);
        for (int i = 0; i < holes.Length; i++)
            holes[i].Prize = ticketPrizes[i];
    }
    private List<T> Shuffle<T>(List<T> li)
    {
        int size = li.Count - 1;
        for (int i = 0; i <= size; i++)
        {
            int randIndex = Random.Range(i, size);
            T tmp = li[i];
            li[i] = li[randIndex];
            li[randIndex] = tmp;
        }
        return li;
    }
}
