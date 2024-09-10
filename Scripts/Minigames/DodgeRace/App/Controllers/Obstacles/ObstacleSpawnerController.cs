using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Events;
using TMPro;

public class ObstacleSpawnerController : MonoBehaviour
{
    public Sprite[] obstacleSprites;
    public Transform passWayParent, obstacleParent;
    public List<GameObject> obstacles, rows, passWays;

    private Coroutine spawnObstacle, increaseSpeed;
    public float  speed;
    public int spawnInterval, increaseSpeedInterval;
    
    private float topBound, bottomBound;
    private void Awake()
    {
        EventRegister();
    }
    
   
    private void Start()
    {
        float rowHeight = rows[0].GetComponent<RectTransform>().rect.height;
        topBound = rowHeight;
        bottomBound = -(rows[0].transform.parent.GetComponent<RectTransform>().rect.height+rowHeight);
        
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(ToggleSpawning);
    }
    private void ToggleSpawning(bool gameState)
    {
        if(gameState)
        {
            GenerateObstacle();
            spawnObstacle = StartCoroutine(TimerController.SetInterval(spawnInterval, GenerateObstacle));
            increaseSpeed = StartCoroutine(TimerController.SetInterval(increaseSpeedInterval, IncreaseSpeed));
        } else
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
    
    
    private void GenerateObstacle()
    {
        GameObject row = GetUnemployedObject(rows);
        List<GameObject> cols = GetAllChildren(row.transform);
        int passWayIndex = GetRandomIndex(cols.Count);
        for(int i=0;i<cols.Count;i++)
        {
            if(i!=passWayIndex)
            {
                GameObject obstacle = GetUnemployedObject(obstacles);
                obstacle.SetActive(true);
                obstacle.GetComponent<Image>().sprite = obstacleSprites[i];
                obstacle.transform.SetParent(cols[i].transform, false);
                obstacle.transform.localPosition = Vector3.zero;
                
            }
            else
            {
                GameObject passWay = GetUnemployedObject(passWays);
                passWay.transform.SetParent(cols[i].transform, false);
                passWay.transform.localPosition = Vector3.zero;
                passWay.SetActive(true);
            }
        }
        row.GetComponent<RowController>().SetVars(speed, bottomBound);
        row.transform.localPosition = new Vector3(0, topBound, 0) ;
        row.SetActive(true);
    }
    private GameObject GetUnemployedObject(List<GameObject> _objects)
    {
        int coba = 0;
        foreach (GameObject obj in _objects)
        {
            if (!obj.activeSelf) return obj;
            coba++;
        }
        
        return null;
    }
    private List<GameObject> GetAllChildren(Transform _parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in _parent)
            children.Add(child.gameObject);
        return children;
    }
    private int GetRandomIndex(int _upBound)
    {
        return UnityEngine.Random.Range(0, _upBound);
    }
    private void IncreaseSpeed()
    {
        speed *= 2;
        if (spawnInterval > 1)
        {
            spawnInterval--;
            StopCoroutine(spawnObstacle);
            spawnObstacle = StartCoroutine(TimerController.SetInterval(spawnInterval, GenerateObstacle));
        }
    }
    public void DispatchRow(GameObject row)
    {
        List<GameObject> cols = GetAllChildren(row.transform);
        foreach(GameObject col in cols)
        {
            Transform colChild = col.transform.GetChild(0);
            colChild.gameObject.SetActive(false);
            if (colChild.gameObject.tag=="PassWay") {
                colChild.SetParent(passWayParent, false);
                continue;
            }
            if(colChild.gameObject.tag=="Obstacle") {
                colChild.SetParent(obstacleParent, false);
                continue;
            }

        }

    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(ToggleSpawning);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }

}
