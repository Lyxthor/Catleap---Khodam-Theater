using UnityEngine;
using UnityEngine.UI;
using Events;

public class BaloonController : MonoBehaviour
{
    public BalloonSpawnerController spawnerController;
    public BalloonScoreController scoreController;
    public string balloonName;
    public float range, maxDistance, speed;
    public float lifeTime = 5;
    private Button button;
    private Vector2 wayPoint;
    private Coroutine live;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBalloonFunc);
        live = StartCoroutine(TimerController.SetPreciseInterval(lifeTime, SelfDestroy));
        EventRegister();
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(OnGameOver);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(OnGameOver);
        
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }

    private void OnClickBalloonFunc()
    {
        int balloonType = spawnerController.IsObjective(balloonName);
        if (balloonType == 0)
        {
            scoreController.MinScore();
            SelfDestroy();
        }
        else if (balloonType == 1)
        {
            scoreController.AddScore();
            SelfDestroy();
        }
        else if (balloonType == -1) GameOver();
        
    }
    private void GameOver()
    {
        EventList.OnChangeGameState.Trigger(false);
    }
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
    private void OnGameOver(bool gameState)
    {
        if(!gameState&&gameObject!=null)
        {
            StopAllCoroutines();
            SelfDestroy();
        }
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, wayPoint) < range)
        {
            wayPoint = SetNextPoint();
        }
    }
    private Vector2 SetNextPoint()
    {
        return new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }
}
