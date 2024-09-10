using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Events;

public class NpcSpawnerController : MonoBehaviour
{
    public GameObject npcParent;
    public GameObject[] Npcs;
    public Transform[] standPositions, seatPositions, doorPositions;
    public int totalSeatsNumber, unlockedSeatsNumber, totalNpc;
    public float visitorPercentage;

    private List<Transform> targetPositions;
    private List<Transform> takenSeatPositions;
    private List<int> testIndex;
    private bool isGoingToSit;
    private Coroutine generateNpc, gatherNpc, dispatchNpc;
    private void Start()
    {
        targetPositions = new List<Transform>();
        takenSeatPositions= new List<Transform>();
        generateNpc=StartCoroutine(Generate());

        gatherNpc = StartCoroutine(TimerController.SetInterval(12, Gather)) ;
        dispatchNpc = StartCoroutine(TimerController.SetInterval(20, Dispatch));
    }
    private void Gather()
    {
        EventList.OnGatherNpc.Trigger();
    }
    private void Dispatch()
    {
        Debug.Log("dispatched");
        EventList.OnDispatchNpc.Trigger();
    }
    private IEnumerator Generate()
    {
        totalNpc = totalSeatsNumber + (int)(totalSeatsNumber * visitorPercentage / 100);
        for (int i = 0; i < totalNpc; i++)
        {
            isGoingToSit = (i <= totalSeatsNumber);
            SetTargetPosition();
            SpawnNpc();
            yield return new WaitForSeconds(1);
        }
        StopCoroutine(generateNpc);
    }
    private void SpawnNpc()
    {
        GameObject newNpc = Instantiate<GameObject>(Npcs[GetRandomNpcIndex()]);
        NpcController npcController = newNpc.GetComponent<NpcController>();


        newNpc.transform.SetParent(npcParent.transform, false);
        newNpc.transform.position = GetRandomDoorPosition().position;
        npcController.SetPositions(targetPositions);
        npcController.SetSitBehaviour(isGoingToSit);
        npcController.spawnerController = this;
        ActivateNpc(newNpc);
    }
    private void ActivateNpc(GameObject npc)
    {
        npc.SetActive(true);
        //FadeNpc(npc);
    }
    private IEnumerator FadeNpc(GameObject npc)
    {
        Image npcImage = npc.GetComponent<Image>();
        Color color = npcImage.color;
        color.a = 0;
        while(color.a < 1)
        {
            color.a += 0.01f;
            npcImage.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private int GetRandomNpcIndex()
    {
        return Random.Range(0, Npcs.Length);
    }
    // MAIN LOGIC
    private void SetTargetPosition()
    {
        targetPositions = new List<Transform>();
        int totalPositions = Random.Range(12, 14);
        for (int i = 0; i < totalPositions; i++)
        {
            if(i==totalPositions-1&&isGoingToSit)
            {
                Transform seatPosition = GetRandomSeatPosition();
                targetPositions.Add(seatPosition);
                takenSeatPositions.Add(seatPosition);
            }
            else
            {
                targetPositions.Add(GetRandomPosition());
            }
        }
    }
    private Transform GetRandomSeatPosition()
    {
        int randomIndex = Random.Range(0, seatPositions.Length);
        if (takenSeatPositions.Count == 0) return seatPositions[randomIndex];
        if (!takenSeatPositions.Contains(seatPositions[randomIndex])) return seatPositions[randomIndex];
        return GetRandomSeatPosition();
    }
    private Transform GetRandomPosition()
    {
        int randomIndex = Random.Range(0, standPositions.Length);
        
        if (targetPositions.Count == 0) return standPositions[randomIndex];
        if (standPositions[randomIndex] != targetPositions.Last())
        {
            Debug.Log(randomIndex);
            return standPositions[randomIndex];
        }
        Debug.Log(randomIndex);
        return GetRandomPosition();
    }

    public Transform GetRandomDoorPosition()
    {
        int randomIndex = Random.Range(0, doorPositions.Length);
        return doorPositions[randomIndex];
    }
    public void GenerateAgain()
    {

    }
}
