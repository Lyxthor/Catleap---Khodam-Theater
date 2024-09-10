using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Events;
public class Npc : MonoBehaviour
{
    public bool viewerStatus;
    public NpcSpawner spawner;
    public Vector3 destination, seatPosition;
    public int moveTime, retreatTime, seatIndex, currentDestinationIndex, countOfDestinationBeforeSit, countOfDestinationBeforeLeave;

    private int destinationVisitedCount;

    
    private NavMeshAgent agent;
    private Coroutine moveDestination;
    private float npcScale;


    private void Start()
    {
        npcScale = transform.localScale.x;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        EventRegister();
    }
    private void Update()
    {
        if (destination.x > transform.position.x) transform.localScale = new Vector3(-npcScale, npcScale, npcScale);
        else transform.localScale = new Vector3(npcScale, npcScale, npcScale);
    }
    private void EventRegister()
    {
        EventList.OnDispatchNpc.Subscribe(Dispatch);
    }
    private void NextDestination()
    {
        if(destinationVisitedCount >= countOfDestinationBeforeLeave)
        {
            Dispatch(false);
        }
        else if(destinationVisitedCount >= countOfDestinationBeforeSit && viewerStatus)
        {
            StopAllCoroutines();
            moveDestination = null;
            destination = seatPosition;
            agent.SetDestination(destination);
        }
        else
        {
            currentDestinationIndex = spawner.GetRandomDestinationIndex(currentDestinationIndex);
            destination = spawner.GetDestination(currentDestinationIndex);
            agent.SetDestination(destination);
        }
        Debug.Log(destinationVisitedCount);
        destinationVisitedCount++;
    }
    private void OnDisable()
    {
        DeactivateNpc();

    }
    private void DeactivateNpc()
    {
        StopAllCoroutines();
        moveDestination = null;

        EventList.OnDispatchNpc.Unsubscribe(Dispatch);
        viewerStatus = false;
        currentDestinationIndex = -1;
        seatPosition = destination = default;
        destinationVisitedCount = 0;
    }
    private void Dispatch(bool _)
    {
        StopAllCoroutines();
        moveDestination = null;

        destination = spawner.GetRandomDoorPosition();
        agent.SetDestination(destination);
        moveDestination = StartCoroutine(TimerController.SetTimeout(retreatTime, delegate { Destroy(gameObject); }));
    }
    public void ActivateNpc()
    {
        gameObject.SetActive(true);
        NextDestination();
        moveDestination = StartCoroutine(TimerController.SetInterval(moveTime, NextDestination));
    }
    private void OnEnable()
    {
        SetAgent();
        agent.SetDestination(destination);
    }
    private void SetAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}
