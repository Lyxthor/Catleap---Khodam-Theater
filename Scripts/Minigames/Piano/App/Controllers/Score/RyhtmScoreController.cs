using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Events;

public class RyhtmScoreController : MonoBehaviour
{
    public TMP_Text scoreText, resultText;
    public MiniGameStatsController statsController;
    public CurtainAnimationController curtainController;
    public TilesEffectView effectView;
    private int totalScore;
    private float perfectTolerance = 0.1f, goodTolerance = 0.5f, hitTolerance = 1;
    private int perfectScore = 4, goodScore = 2, hitScore=1;
    private int auraPrize, totalTiles;
    private StatsModel statsModel;
    private void Start()
    {
        statsModel = new StatsModel();
        EventRegister();
        scoreText.SetText($"{totalScore}");
    }
    private void EventRegister()
    {
        EventList.OnChangeGameState.Subscribe(CalculateScore);
        EventList.OnChangeGameState.Subscribe(SetCurtain);
    }
    private void UnregisterEvent()
    {
        EventList.OnChangeGameState.Unsubscribe(CalculateScore);
        EventList.OnChangeGameState.Unsubscribe(SetCurtain);
    }
    private void OnDestroy()
    {
        UnregisterEvent();
    }
    public bool IsPerfect(Transform origin, Transform _key)
    {
        
        return Vector3.Distance(origin.position, _key.position) <= perfectTolerance;
    }
    public bool IsGood(Transform origin, Transform _key)
    {
        return Vector3.Distance(origin.position, _key.position) <= goodTolerance;
    }
    public bool IsHit(Transform origin, Transform _key)
    {
        return Vector3.Distance(origin.position, _key.position) <= hitTolerance;
    }
    public void ScorePerfect(GameObject toDestroy)
    {
        effectView.ShowHitInfo("PERFECT", Color.yellow);
        UpdateScore(perfectScore);
        Destroy(toDestroy);
    }
    public void ScoreGood(GameObject toDestroy)
    {
        effectView.ShowHitInfo("GOOD", Color.green);
        UpdateScore(goodScore);
        Destroy(toDestroy);
    }
    public void ScoreHit(GameObject toDestroy)
    {
        effectView.ShowHitInfo("HIT", Color.red);
        UpdateScore(hitScore);
        Destroy(toDestroy);
    }
    public void ScoreMiss(GameObject toDestroy)
    {
        effectView.ShowHitInfo("MISS", Color.gray);
        Destroy(toDestroy);
    }
    public void SetScoringPrize(int _auraPrizes, int _totalTiles)
    {
        auraPrize = _auraPrizes;
        totalTiles = _totalTiles;
    }
    private void UpdateScore(int score)
    {
        totalScore += score;
        scoreText.SetText($"{totalScore}");
    }
    
    private void CalculateScore(bool _gameState)
    {
        if(!_gameState)
        {
            List<Dictionary<string, object>> data = statsModel.Get();
            int auraEarn = Mathf.FloorToInt((float) totalScore / totalTiles * auraPrize);
            resultText.SetText($"{totalScore}\n{auraEarn}");
            data[0]["aura"] = (int)data[0]["aura"] + auraEarn;
            statsModel.CreateOrUpdate(data);
        }
    }
    private void SetCurtain(bool gameState)
    {
        if (!gameState) StartCoroutine(TimerController.SetTimeout(2, CurtainDown));
    }
    private void CurtainDown()
    {
        StopAllCoroutines();
        curtainController.MoveDown();
    }
}
