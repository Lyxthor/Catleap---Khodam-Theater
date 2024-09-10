using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Events;
public class NpcController : MonoBehaviour
{
    private List<Transform> positions;
    private Transform target;
    private int positionIndex;
    private bool isGoingToSit, goingToDestroy;
    private NavMeshAgent agent;
    private float timer;
    private float wanderTimer;
    private float npcScale;
    public NpcSpawnerController spawnerController;
    // Start is called before the first frame update
    private void Start()
    {
        SetAgent();
        agent.SetDestination(target.position);
        wanderTimer = 5;
        npcScale = transform.localScale.x;
        /*SetAgent();
        EventRegister();*/

        EventList.OnGatherNpc.Subscribe(ForceGather);
        EventList.OnDispatchNpc.Subscribe(Dispatch);
    }

    private void ForceGather(bool _)
    {
        if (isGoingToSit&&enabled)
        {
            positionIndex = positions.Count - 1;
            target = positions[positionIndex];
            agent.SetDestination(target.position);
        }
    }
    private void Dispatch(bool _)
    {
        enabled = true;
        GenerateNewPositions();
        
    }
    private void SetAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }
    private void EventRegister()
    {

    }
    // Update is called once per frame
    private void Update()
    {

        timer += Time.deltaTime;
        if (timer > wanderTimer) NextPosition();
        if (target.position.x > transform.position.x) transform.localScale = new Vector3(-npcScale, npcScale, npcScale);
        else transform.localScale = new Vector3(npcScale, npcScale, npcScale);
    }

    private bool IsReached()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.001f)
            {
                return true;
            }
        }
        return false;
    }
    private void NextPosition()
    {
        
        Debug.Log("pindah posisi");
        //if (goingToDestroy) Destroy(gameObject);
        if (positionIndex == positions.Count - 1 && isGoingToSit)
        {
            enabled = false;
        }
        else if (positionIndex == positions.Count - 1 && !isGoingToSit)
        {
            GenerateNewPositions();
        }
        else
        {
            timer = 0;
            positionIndex++;
            target = positions[positionIndex];
            agent.SetDestination(target.position);
        }
    }
    private void GenerateNewPositions()
    {
        target = spawnerController.GetRandomDoorPosition();
        goingToDestroy = true;
        agent.SetDestination(target.position);
    }

    public void SetPositions(List<Transform> _positions)
    {
        positions = _positions;
        target = positions[0];
        
    }
    public void SetSitBehaviour(bool _isGoingToSit)
    {
        isGoingToSit = _isGoingToSit;
    }
}
