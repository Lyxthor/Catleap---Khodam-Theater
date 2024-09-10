using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class CarPlayerController : MonoBehaviour
{
    public DodgeCarScoreController scoreController;
    public float speed;
    private float xInput, yInput, xMovement, yMovement;
    private Rigidbody2D body;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        xMovement = speed * xInput;
        yMovement = speed * yInput;
        body.velocity = new Vector3(xMovement, yMovement, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PassWay")
            scoreController.UpdateScore(1);
        else if (collision.gameObject.tag == "Obstacle")
            TriggerGameOver();
    }
    private void TriggerGameOver()
    {
        EventList.OnChangeGameState.Trigger(false);
        Destroy(gameObject);
    }
}
