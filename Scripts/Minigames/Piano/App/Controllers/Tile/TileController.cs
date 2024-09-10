using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public RyhtmScoreController scoreController;
    public Transform key;
    public KeyCode keyCode;
    public float bound;
    private float speed = 2, direction=-1;
    
    
    private void Update()
    {
        if(Input.GetKey(keyCode))
        {
            
            if (scoreController.IsPerfect(transform, key))
            {
                scoreController.ScorePerfect(gameObject);
                return;
            }
            if (scoreController.IsGood(transform, key)) 
            {
                scoreController.ScoreGood(gameObject);
                return;
            }
            if(scoreController.IsHit(transform, key))
            {
                scoreController.ScoreHit(gameObject);
                return;
            }
        }
        if (transform.position.x < bound)
        {
            scoreController.ScoreMiss(gameObject);
        }
        MoveTile();
    }
    private void MoveTile()
    {
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0, Space.World);
    }
    
}
