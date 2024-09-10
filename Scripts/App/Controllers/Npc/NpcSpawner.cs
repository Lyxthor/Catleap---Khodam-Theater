using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class NpcSpawner : MonoBehaviour
{

    
    public int maxWaveAmount;
    public Transform npcParent;
    public float waveDurations;
    public int viewerNpcPercentagePerWave, retreatTime, maxNpcAmount;
    public List<Transform> destinations, seatPositions, doorPositions;
    public List<GameObject> npcs;

    private System.Random rand;
    private List<int> takenSeatPositionIndexs;
    private int  currentWave, npcAmount, viewerNpcAmount;
    private float npcPercentagePerWave, spawnNpcInterval;
    private int currentViewerCount;


    private Coroutine nextWave, spawnNpc, initWave;
    private void Start()
    {
        rand = new System.Random();
        currentWave = 1;
        Init();

    }
    private void Init()
    {
        currentViewerCount = 0;
        takenSeatPositionIndexs = new List<int>();
        CalculateSpawnerVariables();
        spawnNpc = StartCoroutine(TimerController.SetPreciseInterval(spawnNpcInterval, SpawnNpc));
        nextWave = StartCoroutine(TimerController.SetPreciseInterval(waveDurations, NextWave));
        
    }
    private void NextWave()
    {
        Debug.Log("New Wave");
        StopAllCoroutines();
        currentWave = currentWave < maxWaveAmount ? currentWave + 1 : 1;
        RetreatNpc();
        initWave = StartCoroutine(TimerController.SetTimeout(retreatTime, Init));
    }
    private void RetreatNpc()
    {
        StopAllCoroutines();
        EventList.OnDispatchNpc.Trigger();
    }
    private void CalculateSpawnerVariables()
    {
        npcPercentagePerWave = (float)currentWave / maxWaveAmount;
        npcAmount = Mathf.RoundToInt(npcPercentagePerWave * maxNpcAmount);
        viewerNpcAmount = Mathf.RoundToInt((float)viewerNpcPercentagePerWave / 100 * npcAmount);
        spawnNpcInterval = waveDurations / npcAmount;

       
    }
    public Vector3 GetDestination(int index)
    {
        return destinations[index].position;
    }
    public void EmptySeat(int seatIndex)
    {
        takenSeatPositionIndexs.Remove(seatIndex);
    }
    public int GetRandomDestinationIndex(int currentIndex=-1)
    {
        int newDestinationIndex = rand.Next(0, destinations.Count);
        if (newDestinationIndex != currentIndex) return newDestinationIndex;
        return GetRandomDestinationIndex(currentIndex);
    }
    private int GetRandomSeatIndex()
    {
        int randomSeatIndex = rand.Next(0, seatPositions.Count);
        if (!takenSeatPositionIndexs.Contains(randomSeatIndex)) return randomSeatIndex;
        return GetRandomSeatIndex();
    }
    public Vector3 GetRandomDoorPosition()
    {
        int randomDoorIndex = rand.Next(0, doorPositions.Count);
        return doorPositions[randomDoorIndex].position;
    }
    
    private void SpawnNpc()
    {
        GameObject npc = Instantiate(npcs[rand.Next(0, npcs.Count)]);
        bool viewerStatus = currentViewerCount < viewerNpcAmount;
        
       
        Npc npcController = npc.GetComponent<Npc>();
        npc.transform.SetParent(npcParent, false);
        npc.transform.position = GetRandomDoorPosition();
        npcController.moveTime = 5;
        npcController.retreatTime = retreatTime;
        npcController.viewerStatus = viewerStatus;
        npcController.currentDestinationIndex = -1;
        npcController.spawner = this;
        
        if(viewerStatus)
        {
            int randomSeatIndex = GetRandomSeatIndex();
            npcController.seatIndex = randomSeatIndex;
            npcController.seatPosition = seatPositions[randomSeatIndex].position;
            takenSeatPositionIndexs.Add(randomSeatIndex);
            currentViewerCount++;
        }
        npcController.ActivateNpc();
        
    }
}
