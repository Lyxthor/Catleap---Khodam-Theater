using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class NpcSpawner2 : MonoBehaviour
{
/*
    
    public int maxWaveAmount, maxNpcAmount;
    public float waveDurations;
    public int viewerNpcPercentagePerWave, retreatTime;
    public List<Transform> destinations, seatPositions, doorPositions;
    public List<GameObject> npcs;

    private List<int> takenSeatPositionIndexs;
    private int  currentWave,  npcAmount, viewerNpcAmount;
    private float npcPercentagePerWave, spawnNpcInterval;
    private int currentActiveNpcIndex;
    private List<GameObject> activeNpcs;

    private Coroutine nextWave, spawnNpc, initWave;
    private void Start()
    {
        takenSeatPositionIndexs = new List<int>();
        currentWave = 1;
        maxNpcAmount = npcs.Count;
        Init();
    }
    private void Init()
    {
        currentActiveNpcIndex = 0;
        activeNpcs = new List<GameObject>();
        CalculateSpawnerVariables();
        SetActiveNpcs();
        ShuffleActiveNpcs();
        spawnNpc = StartCoroutine(TimerController.SetPreciseInterval(spawnNpcInterval, SpawnNpc));
        nextWave = StartCoroutine(TimerController.SetPreciseInterval(waveDurations, NextWave));
    }
    private void NextWave()
    {
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

        Debug.Log(npcPercentagePerWave);
        Debug.Log(npcAmount);
        Debug.Log(viewerNpcAmount);
        Debug.Log(spawnNpcInterval);
    }
    public void DeactivateNpc(GameObject npc)
    {
        activeNpcs.Remove(npc);
    }
    private void SetActiveNpcs()
    {
        for (int i=0;i<npcAmount;i++)
        {
            
            bool isViewer = i < viewerNpcAmount;
            GameObject npc = npcs[Random.Range(0, maxNpcAmount - 1)];
            if (npc.activeSelf) continue;
            Npc npcController = npc.GetComponent<Npc>();

            int randomDestinationIndex = GetRandomDestinationIndex();
            npc.transform.position = GetRandomDoorPosition();
            npcController.spawner = this;
            npcController.viewerStatus = isViewer;
            npcController.currentDestinationIndex = randomDestinationIndex;
            npcController.destination = (destinations[randomDestinationIndex].position);
            if (isViewer)
            {
                int randomSeatIndex = GetRandomSeatIndex();
                npcController.seatIndex = randomSeatIndex;
                npcController.seatPosition = seatPositions[randomSeatIndex].position;
                takenSeatPositionIndexs.Add(randomSeatIndex);
            }
            else
            {
                npcController.seatIndex = -1;
            }
            activeNpcs.Add(npc);
        }
        Debug.Log(activeNpcs.Count);
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
        int newDestinationIndex = Random.Range(0, destinations.Count - 1);
        if (newDestinationIndex != currentIndex) return newDestinationIndex;
        return GetRandomDestinationIndex(currentIndex);
    }
    private int GetRandomSeatIndex()
    {
        int randomSeatIndex = Random.Range(0, seatPositions.Count - 1);
        if (!takenSeatPositionIndexs.Contains(randomSeatIndex)) return randomSeatIndex;
        return GetRandomSeatIndex();
    }
    public Vector3 GetRandomDoorPosition()
    {
        int randomDoorIndex = Random.Range(0, doorPositions.Count - 1);
        return doorPositions[randomDoorIndex].position;
    }
    
    private void ShuffleActiveNpcs()
    {
        for(int i=0;i<activeNpcs.Count;i++)
        {
            int randomIndex = Random.Range(0, activeNpcs.Count - 1);
            GameObject shuffledNpc = activeNpcs[i];
            activeNpcs[i] = activeNpcs[randomIndex];
            activeNpcs[randomIndex] = shuffledNpc;
        }
    }
    private void SpawnNpc()
    {
        GameObject spawnedNpc = activeNpcs[currentActiveNpcIndex];
        spawnedNpc.SetActive(true);
        spawnedNpc.GetComponent<Npc>().Init();
        currentActiveNpcIndex++;
    }*/
}
