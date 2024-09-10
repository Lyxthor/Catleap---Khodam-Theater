using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Events;

[System.Serializable]
public class BalloonKeyPair
{
    public string name;
    public Sprite sprite;
}
public class BalloonSpawnerController : MonoBehaviour
{
    public GameObject balloonPrefab, objectiveIconPrefab;
    public Transform objectivesBar;
    public List<BalloonKeyPair> balloonKeyPairs;
    public float spawnInterval;

    private int score;
    private string bombName;
    private List<string> objectiveBalloons;
    private Coroutine spawnBalloon;
    
    private void Start()
    {
        objectiveBalloons = new List<string>();
        bombName = balloonKeyPairs.Last().name;
        InitObjectives();
        EventRegister();
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(StartSpawn);
        EventList.OnChangeGameState.Subscribe(EndSpawn);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(StartSpawn);
        EventList.OnChangeGameState.Unsubscribe(EndSpawn);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }
    private void StartSpawn(bool gameState)
    {
        if(gameState) spawnBalloon = StartCoroutine(TimerController.SetPreciseInterval(spawnInterval, SpawnBalloon));
    }
    private void EndSpawn(bool gameState)
    {
        if (!gameState) StopAllCoroutines();
    }
    private void InitObjectives()
    {
        int numberOfObjectives = 2;
        for(int i=0;i<numberOfObjectives;i++)
        {
            InsertRandomBalloonAsObjective();
        }
    }
    private void InsertRandomBalloonAsObjective()
    {
        int randomIndex = Random.Range(0, balloonKeyPairs.Count-1);
        Debug.Log(balloonKeyPairs[randomIndex].name);
        if (objectiveBalloons.Contains(balloonKeyPairs[randomIndex].name)) InsertRandomBalloonAsObjective();
        else
        {
            objectiveBalloons.Add(balloonKeyPairs[randomIndex].name);
            GameObject newObjectiveIcon = Instantiate(objectiveIconPrefab);
            newObjectiveIcon.GetComponent<Image>().sprite = balloonKeyPairs[randomIndex].sprite;
            newObjectiveIcon.transform.SetParent(objectivesBar, false);
            newObjectiveIcon.SetActive(true);
        }
    }
    private void SpawnBalloon()
    {
        BalloonKeyPair bkPair = balloonKeyPairs[Random.Range(0, balloonKeyPairs.Count)];
        Vector2 spawnPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        GameObject newBalloon = Instantiate(balloonPrefab);
        Image newBalloonImage = newBalloon.GetComponent<Image>();
        newBalloonImage.sprite = bkPair.sprite;
        newBalloon.GetComponent<BaloonController>().balloonName = bkPair.name;
        newBalloon.transform.SetParent(balloonPrefab.transform.parent, false);
        newBalloon.transform.position = spawnPosition;
        newBalloon.SetActive(true);
    }
    public int IsObjective(string balloonName)
    {
        if (objectiveBalloons.Contains(balloonName)) return 1;
        if (balloonName == bombName) return -1;
        return 0;
    }
    
}
