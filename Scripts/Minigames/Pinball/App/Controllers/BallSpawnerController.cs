using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawnerController : MonoBehaviour
{
    public GameObject ball, ballParent;
    public Button spawnerButton;
    public BallShopController ballShopController;
    private void Start()
    {
        spawnerButton.onClick.AddListener(SpawnBall);
    }
    private void SpawnBall()
    {
        int ballAmounts = ballShopController.GetCurrentBallAmount();
        if (ballAmounts > 0)
        {
            GameObject newBall = CreateNewBall();
            newBall.SetActive(true);
            ballAmounts--;
            ballShopController.UpdateBall(ballAmounts);
        }
    }
    private GameObject CreateNewBall()
    {
        Vector3 spawnPoint = GetBallSpawnPosition();
        GameObject newBall = Instantiate<GameObject>(ball);
        newBall.transform.SetParent(ballParent.transform, false);
        newBall.transform.position = spawnPoint;
        return newBall;
    }
    private Vector3 GetBallSpawnPosition()
    {
        Vector3 spawnPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(spawnPoint.x, spawnPoint.y, 0);
    }
}
