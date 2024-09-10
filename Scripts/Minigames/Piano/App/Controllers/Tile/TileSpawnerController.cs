using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Events;



public class TileSpawnerController : MonoBehaviour
{
    public int totalTilesPercentage;
    public PerformVideoController videoController;
    public LaneIconPair[] laneIconPairs;
    public Transform[] keys;
    public GameObject blanktile;
    public RyhtmScoreController scoreController;

    private string performFileName;
    private float leftBound, rightBound;
    private List<KeyCode> keyCodes;
    private PerformData performData;
    private List<Coroutine> timestamps;
    private List<GameObject> tiles;

    private float tileInterval;

    private void Start()
    {
        tiles = new List<GameObject>();
        timestamps = new List<Coroutine>();
        performFileName = (string) StageInfoCourier.Instance.stageData["perform_data"];
        leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x;
        rightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x;
        performData = Resources.Load<PerformData>($"Object/{performFileName}");
        keyCodes = new List<KeyCode> { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
        videoController.SetVideoClip(performData.videoClip);
        tileInterval = (float) videoController.GetTileInterval();

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
    private void Init()
    {
        int index = timestamps.Count;
        int totalSpawningTiles = Mathf.RoundToInt((float)totalTilesPercentage / 100 * videoController.totalTiles);
        scoreController.SetScoringPrize(performData.auraPrize, totalSpawningTiles);
        for (int i=0;i < totalSpawningTiles;i++)
        {
            /*tiles.Add(CreateTile(timeLane.lane));*/
            /*timestamps.Add(StartCoroutine(SpawnTile(timeLane.time, index)));*/
            int randomLane = UnityEngine.Random.Range(0, 4);
            tiles.Add(CreateTile(randomLane));
            timestamps.Add(StartCoroutine(SpawnTile(tileInterval * (index+1), index)));
            index++;
        }
    }
    private void StartSpawn(bool _gameState)
    {
        if(_gameState)
        {
            Init();
            videoController.StartPlay();
        }
    }
    private void EndSpawn(bool _gameState)
    {
        if(!_gameState)
        {
            StopAllCoroutines();
        }
    }
    private IEnumerator SpawnTile(float waitTime, int index)
    {
        yield return new WaitForSeconds(waitTime);

        tiles[index].SetActive(true);
        StopCoroutine(timestamps[index]);
    }
    private GameObject CreateTile(int lane)
    {
        GameObject newTile = Instantiate(blanktile);
        AssignIconLane(newTile, lane);
        return newTile;
    }
    private void AssignIconLane(GameObject _newTile, int laneIndex)
    {
        Transform lane = laneIconPairs[laneIndex].lane.transform;
        TileController tileController = _newTile.GetComponent<TileController>();
        _newTile.transform.position = new Vector3(rightBound, 0, 0);
        _newTile.transform.SetParent(lane, false);
        _newTile.GetComponent<Image>().sprite = laneIconPairs[laneIndex].icon;
        tileController.key = keys[laneIndex];
        tileController.keyCode = keyCodes[laneIndex];
        tileController.bound = leftBound;
    }
   



    [Serializable]
    public class LaneIconPair
    {
        public Sprite icon;
        public GameObject lane;
    }
}
