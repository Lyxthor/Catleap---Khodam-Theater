using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoCourier : MonoBehaviour
{
    private static StageInfoCourier _instance;
    public static StageInfoCourier Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find an existing instance of the class in the scene
                _instance = FindObjectOfType<StageInfoCourier>();

                if (_instance == null)
                {
                    // Create a new GameObject and add the SingletonExample component
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<StageInfoCourier>();
                    singletonObject.name = typeof(StageInfoCourier).ToString() + " (Singleton)";
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
        }
    }

    public Dictionary<string, object> stageData;
    public bool isStageEnabled=false;
}
