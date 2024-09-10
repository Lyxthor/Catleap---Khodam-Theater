using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowController : MonoBehaviour
{
    public ObstacleSpawnerController spawnerController;
    private float speed, bottomBound;
    private void Update()
    {
        float movementSpeed = spawnerController.speed * Time.deltaTime * -1;
        transform.Translate(0, movementSpeed, 0);
        if (IsReachBottom()) Deactivate();
    }
    private bool IsReachBottom()
    {
        
        return transform.localPosition.y < bottomBound;
    }
    private void Deactivate()
    {
        spawnerController.DispatchRow(gameObject);
        gameObject.SetActive(false);
    }
    public void SetVars(float _speed, float _bottomBound)
    {
        speed = _speed;
        bottomBound = _bottomBound;
    }
}
