using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using Connections;

public class SceneController : MonoBehaviour
{
    public GameObject sceneTransition;
    private Dictionary<string, List<Action>> sceneActions;
    private static SceneController _instance;
    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find an existing instance of the class in the scene
                _instance = FindObjectOfType<SceneController>();

                if (_instance == null)
                {
                    // Create a new GameObject and add the SingletonExample component
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SceneController>();
                    singletonObject.name = typeof(SceneController).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            sceneActions = new Dictionary<string, List<Action>>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneLoaded += PlayTheme;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas cv = GetComponent<Canvas>();
        cv.worldCamera = Camera.main;
        if(sceneTransition.activeSelf)
        {
            PlaySceneTransition("BurnOut");
            StartCoroutine(TimerController.SetTimeout(4, StopSceneTransition));
        }
    }
    private void PlayTheme(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LoadScene" || scene.name == "RythmGame") 
        {
            if (AudioManager.Instance.musicSource.isPlaying) AudioManager.Instance.StopMusic();
            return;
        }
        if (scene.name!="MainMenu" && AudioManager.Instance.MusicSourceName == "gameplay_theme" && AudioManager.Instance.musicSource.isPlaying) return;
        

            
        if(scene.name=="MainMenu")
        {
            AudioManager.Instance.PlayMusic("menu_theme");
        } else
        {
            AudioManager.Instance.PlayMusic("gameplay_theme");
        }
    }
    public void BackToGamePlay(bool withTransition=false)
    {
        LoadByName("GamePlay", withTransition);
    }
    private void RedirectScene(string name)
    {
        LoadByName(name);
        StopAllCoroutines();
    }
    public void LoadByName(string name, bool withTransition=false)
    {
        if (withTransition)
        {
            PlaySceneTransition("BurnIn");
            StartCoroutine(TimerController.SetTimeout(2, delegate { RedirectScene(name); }));
        }
        else
        {
            SceneManager.LoadSceneAsync(name);
        }
    }
    
    public void LoadByIndex(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }
    public void NextScene()
    {

    }
    public void PrevScene()
    {

    }
    public void MainMenu()
    {
        Debug.Log("Coba");
        LoadByName("MainMenu");
    }
    public void GameQuit()
    {
    #if UNITY_EDITOR
            // Stop playing the game in the Editor
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            // Quit the application in a standalone build
            Application.Quit();
    #endif
        SqliteDbConnection.Disconnect();
        Debug.Log("COME OUT");
    }
    public void PlaySceneTransition(string _transition)
    {
        sceneTransition.SetActive(true);
        Animator transitionAnim = sceneTransition.GetComponent<Animator>();
        transitionAnim.SetTrigger(_transition);
    }
    public void StopSceneTransition()
    {
        sceneTransition.SetActive(false);
        StopAllCoroutines();
    }
    public void SubscribeSceneLoaded(string sceneName, Action action)
    {
        if(!sceneActions.ContainsKey(sceneName)) sceneActions.Add(sceneName, new List<Action>());
        sceneActions[sceneName].Add(action);
        
    }
    private void LoadScenesActions(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneActions[sceneName].Count);
        if (!sceneActions.ContainsKey(sceneName)) return;
        foreach(Action action in sceneActions[sceneName])
        {
            action?.Invoke();
        }
    }
    private class PagesMusicPair
    {
        public string music;
        public List<string> pages;

        public PagesMusicPair(string _music, List<string> _pages)
        {
            music = _music;
            pages = _pages;
        }
    }
}
