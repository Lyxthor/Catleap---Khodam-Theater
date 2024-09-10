using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayzoneParentController : MonoBehaviour
{


    private List<Dictionary<string, object>> data;
    public PlayzoneController[] childControllers;
    private PlayzoneModel model; // THIS MODEL USES SQLITEMODEL, SO BE AWARE OF THE DATA TYPES
    
    private void Awake()
    {
        model = new PlayzoneModel();
        GetData();
        
    }
    private void OnEnable()
    {
        if(data==null)
        {
            model = new PlayzoneModel();
            GetData();
        }
    }

    private void GetData()
    {
        data = model.All();
        SendDataToChild();
    }
    private void SendDataToChild()
    {
        if (data == null) return;
        if (data.Count == 0) return;

        for(int i=0;i<data.Count;i++) {
            childControllers[i].SetData(data[i]);
        }
    }
    

}
