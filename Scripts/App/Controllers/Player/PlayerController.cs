using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float playerScale;
    private Animator anim;
    private Rigidbody2D body;
    private void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        playerScale = transform.localScale.x;
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
        body.velocity = new Vector2(horizontalInput * 2, verticalInput*2);
        anim.SetBool("IsWalk", (horizontalInput != 0 || verticalInput !=0));
    }
}
