using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
public interface Prize
{
    public string ImagePath { get; }
    public string VideoPath { get; }
    public string TitleText { get; }
    public bool IsAvaiable();
    public bool UseImage();
    public void Gift();
    public int GiveAway();
}
public class AuraPrize : Prize
{
    private int prizeAmount;
    private StatsController globalStatsController;
    public string ImagePath { get => "Icons/aura"; }
    public string VideoPath { get => "GachaVideos/aura_prize"; }
    public string TitleText { get => $"{prizeAmount}"; }
    public bool IsAvaiable()
    {
        return true;
    }
    public void Gift()
    {
        
    }
    public bool UseImage()
    {
        return true;
    }
    public int GiveAway()
    {
        return prizeAmount;
    }
    public static List<Prize> CreateAuraPrizes(int[] auraPrizeAmounts)
    {
        List<Prize> newAuraPrizes = new List<Prize>();
        foreach (int auraPrizeAmount in auraPrizeAmounts)
        {
            AuraPrize newAuraPrize = new AuraPrize(auraPrizeAmount);
            newAuraPrizes.Add(newAuraPrize);
        }
        return newAuraPrizes;
    }
    public AuraPrize(int _auraPrizeAmount)
    {
        prizeAmount = _auraPrizeAmount;
    }
}
public class StoryPrize : Prize
{
    private Dictionary<string, object> data;
    private StoryModel model;
    private bool isAvaiable = true;
    public string ImagePath { get => (string)data["image_url"]; }
    public string VideoPath { get => (string)data["video_url"]; }
    public string TitleText { get => $"{data["story_title"]}"; }
    public static List<Prize> CreateStoryPrizes(List<Dictionary<string, object>> storyData)
    {
        List<Prize> newAuraPrizes = new List<Prize>();
        foreach (Dictionary<string, object> storyEntry in storyData)
        {
            StoryPrize newStoryPrize = new StoryPrize(storyEntry);
            newAuraPrizes.Add(newStoryPrize);
        }
        return newAuraPrizes;
    }
    public bool IsAvaiable()
    {
        bool avaiable = isAvaiable;
        isAvaiable = false;
        return avaiable;
    }
    public bool UseImage()
    {
        return false;
    }
    public void Gift()
    {
        int index = Random.Range(0, data.Count);
        data["active_state"] = "unlocked";

        model = new StoryModel();
        model.Update(data, (int)(long)data["id"]);
        // UPDATE 
    }
    public int GiveAway()
    {
        return 0;
    }
    public StoryPrize(Dictionary<string, object> _data)
    {
        data = _data;
    }
}

public class GachaController : MonoBehaviour
{
    public TMP_Text resultText;
    public Button onePull, bulkPull, nextButton, skipButton;
    public GameObject gachaResultPanel;
    public List<Image> gachaResultImages;
    public List<TMP_Text> titleText;
    [SerializeField] private float commonPercentage = 50f;      // Default 50%
    [SerializeField] private float rarePercentage = 30f;    // Default 30%
    [SerializeField] private float epicPercentage = 15f;        // Default 15%
    [SerializeField] private float legendPercentage = 5f;    // Default 5%

    [SerializeField] private int totalDraws = 100;              // Total number of draws

    [SerializeField] private int minDrawForRare = 5;       // Minimum draws to get rare
    [SerializeField] private int minDrawsForEpic = 20;          // Minimum draws to get rare
    [SerializeField] private int minDrawForLegend = 50;     // Minimum draws to get ultra rare

    [SerializeField] private int[] commonAuraPrizeAmount;
    [SerializeField] private int[] rareAuraPrizeAmount;
    [SerializeField] private int[] epicAuraPrizeAmount;
    [SerializeField] private int[] ultraRareAuraPrizeAmount;

    private GachaVideoController gachaVideoController;

    private StoryModel model;

    private List<string> gachaPool;
    private List<Dictionary<string, object>> storyData;
    private Dictionary<string, List<Prize>> prizes;
    private List<Prize> result;
    private int currentDrawCount = 0;

    void Start()
    {
        model = new StoryModel();
        storyData = model.Where("active_state","locked");
        gachaVideoController = GetComponent<GachaVideoController>();
        InitializePrizes();
        InitializeGachaPool();
        Shuffle(gachaPool);
        /*onePull.onClick.AddListener(delegate { Roll(1); });
        bulkPull.onClick.AddListener(delegate { Roll(10); });
        nextButton.onClick.AddListener(LoadNextPrize);
        skipButton.onClick.AddListener(ShowResult);*/
    }
    public void UpdatePrizeData()
    {
        storyData = model.Where("active_state","locked");
        
        InitializePrizes();
    }
    private void InitializeGachaPool()
    {
        gachaPool = new List<string>();
        int commonCount = Mathf.RoundToInt(commonPercentage / 100f * totalDraws);
        int rareCount = Mathf.RoundToInt(rarePercentage / 100f * totalDraws);
        int epicCount = Mathf.RoundToInt(epicPercentage / 100f * totalDraws);
        int ultraRareCount = Mathf.RoundToInt(legendPercentage / 100f * totalDraws);
        gachaPool.AddRange(CreateList("common", commonCount));
        gachaPool.AddRange(CreateList("rare", rareCount));
        gachaPool.AddRange(CreateList("epic", epicCount));
        gachaPool.AddRange(CreateList("legend", ultraRareCount));
    }

    private List<string> CreateList(string rarity, int count)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < count; i++)
            list.Add(rarity);
        return list;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private string DrawGacha()
    {
        currentDrawCount++;
        List<string> filteredPool = new List<string>();
        foreach (var item in gachaPool)
        {
            if (item == "legend" && currentDrawCount >= minDrawForLegend)
                filteredPool.Add(item);
            else if (item == "epic" && currentDrawCount >= minDrawsForEpic)
                filteredPool.Add(item);
            else if (item == "rare" && currentDrawCount >= minDrawForRare)
                filteredPool.Add(item);
            else if (item == "common")
                filteredPool.Add(item);
        }
        Shuffle(filteredPool);
        string result = filteredPool[0];
        gachaPool.Remove(result); 
        return result;
    }
    public List<Prize> Roll(int rollAmount)
    {
        result = new List<Prize>();
        if (currentDrawCount == totalDraws) ResetGacha();
        for (int i = 0; i < rollAmount; i++)
            SetPrizesResult();
        return result;
        /*gachaVideoController.WaitVideoToEnd(PlayFirstClip);
        gachaVideoController.SetUp("GachaVideos/gacha_opening");*/
    }
    private void PlayFirstClip()
    {
        //gachaVideoController.InitTitleImageTransition();
        
        LoadNextPrize();
    }
    private void SetPrizesResult()
    {
        string rarity = DrawGacha();
        int prizeIndex = Random.Range(0, prizes[rarity].Count);
        Prize prize = prizes[rarity][prizeIndex];
        //Debug.Log(prize.VideoPath);
        if (prizes[rarity].Count > 0 && prize.IsAvaiable()) result.Add(prize);
        else SetPrizesResult();
    }
    private void InitializePrizes()
    {

        prizes = new Dictionary<string, List<Prize>>();
        AddNewPrize("common", AuraPrize.CreateAuraPrizes(commonAuraPrizeAmount));
        AddNewPrize("rare", AuraPrize.CreateAuraPrizes(rareAuraPrizeAmount));
        AddNewPrize("epic", AuraPrize.CreateAuraPrizes(rareAuraPrizeAmount));
        AddNewPrize("legend", AuraPrize.CreateAuraPrizes(ultraRareAuraPrizeAmount));
        AddNewPrize("common", StoryPrize.CreateStoryPrizes(storyData.Where(s => (string)s["rarity"] == "common").ToList()));
    }
    private int clipIndex = 0;
    private void LoadNextPrize()
    {
        
        
        if (clipIndex == 0) gachaVideoController.UnWaitVideoToEnd(PlayFirstClip);
        if (clipIndex == result.Count - 1) gachaVideoController.WaitVideoToEnd(ShowResult);
        if (clipIndex == result.Count)
        {
            ShowResult();
            gachaVideoController.StopVideo();
            return;
        }
        string videoPath = result[clipIndex].VideoPath;
        string imagePath = result[clipIndex].ImagePath;
        string titleText = result[clipIndex].TitleText;
        bool useImage = result[clipIndex].UseImage();
        
        gachaVideoController.PlayGachaAnimation(videoPath, imagePath, titleText, useImage);
        clipIndex++;
        
        
    }
    private void ShowResult()
    {
        clipIndex = 0;
        for(int i=0;i<result.Count;i++)
        {
            Prize prize = result[i];
            Texture2D texture = Resources.Load<Texture2D>(prize.ImagePath);
            if (texture != null) { 
                gachaResultImages[i].sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
                titleText[i].SetText($"{prize.GiveAway()}");
            }
        }
        gachaResultPanel.SetActive(true);
    }
    private void AddNewPrize(string rarity, List<Prize> prize)
    {
        if (!prizes.ContainsKey(rarity)) prizes.Add(rarity, prize);
        else prizes[rarity].AddRange(prize);
    }
    private void ResetGacha()
    {
        currentDrawCount = 0;
        InitializeGachaPool();
        Shuffle(gachaPool);
    }
    

    

}