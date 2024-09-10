using UnityEngine;
using UnityEngine.AI;
using Events;

public class Npc2 : MonoBehaviour
{
   /* public bool viewerStatus;
    public NpcSpawner spawner;
    public Vector3 destination, seatPosition;
    public int seatIndex, currentDestinationIndex, countOfDestinationBeforeSit, countOfDestinationBeforeLeave;

    private int destinationVisitedCount;
    
    private int lifeTime;
    private NavMeshAgent agent;
    private Coroutine moveDestination;
    private float npcScale;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }
    private void Update()
    {
        if (destination.x > transform.position.x) transform.localScale = new Vector3(-npcScale, npcScale, npcScale);
        else transform.localScale = new Vector3(npcScale, npcScale, npcScale);
    }
    public void Init()
    {
        if(agent==null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateUpAxis = false;
            agent.updateRotation = false;
        }
        lifeTime = 5;
        npcScale = transform.localScale.x;
        SetDestination(destination);
        EventRegister();
        StartMove();
    }
    private void StartMove()
    {
        moveDestination = StartCoroutine(TimerController.SetInterval(lifeTime, GetNewDestination));
    }
    private void EventRegister()
    {
        EventList.OnDispatchNpc.Subscribe(Dispatch);
    }
    
    private void GetNewDestination()
    {
        destinationVisitedCount++;
        if(destinationVisitedCount >= countOfDestinationBeforeLeave)
        {
            GoHome();
        }
        else if(destinationVisitedCount >= countOfDestinationBeforeSit&&viewerStatus)
        {
            moveDestination = null;
            StopAllCoroutines();
            SetDestination(seatPosition);
        }
        else
        {
            currentDestinationIndex = spawner.GetRandomDestinationIndex(currentDestinationIndex);
            Vector3 newDestination = spawner.GetDestination(currentDestinationIndex);
            SetDestination(newDestination);
        }
    }
    private void Dispatch(bool _)
    {
        if(destinationVisitedCount <  countOfDestinationBeforeLeave)
        {
            GoHome();
        }
    }
    private void GoHome()
    {
        StopAllCoroutines();
        Vector3 doorPosition = spawner.GetRandomDoorPosition();
        SetDestination(doorPosition);
        moveDestination = StartCoroutine(TimerController.SetTimeout(5, SelfDestruct));
    }
    private void SelfDestruct()
    {
        
        ResetVariables();
        gameObject.SetActive(false);
        spawner.EmptySeat(seatIndex);
        spawner.DeactivateNpc(gameObject);
    }
   
    public void SetDestination(Vector3 _destination)
    {
        destination = _destination;
        agent.SetDestination(destination);
    }
    private void ResetVariables()
    {
        EventList.OnDispatchNpc.Unsubscribe(Dispatch);

        
        StopAllCoroutines();
        moveDestination = null;
        viewerStatus = false;
        destinationVisitedCount = 0;
        seatIndex = -1;
        seatPosition = default;
        moveDestination = null;
    }
    private void OnDisable()
    {
        ResetVariables();
    }*/
}
