using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public MiniGameStatsController miniGameStatsController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsReachedHole(collision))
            miniGameStatsController.UpdateTicket(GetPrize(collision));
        
        /*if (!(collision.gameObject.tag == "Hole")) return;
        if (reachedHole) return;
        // add ticket
        HolePrize holePrize = collision.gameObject.GetComponent<HolePrize>();
        int ticketPrizeAmounts = holePrize.TicketPrize;
        Debug.Log(ticketPrizeAmounts);
        shop.ticketAmounts += ticketPrizeAmounts;
        shop.UpdateText();
        Destroy(this);*/


    }
    private bool IsReachedHole(Collider2D collision)
    {
        return collision.gameObject.tag == "Hole";
    }
    private int GetPrize(Collider2D collision)
    {
        HoleController prize = collision.gameObject.GetComponent<HoleController>();
        return prize.Prize;
    }
}
