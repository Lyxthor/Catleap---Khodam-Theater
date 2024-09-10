using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnualPaymentParentController : MonoBehaviour
{
    private AnnualPaymentModel model;
    private void Start()
    {
        model = new AnnualPaymentModel();
    }
    public void Get()
    {
        List<Dictionary<string, object>> data = model.All();
    }
    public void Show(int id)
    {
        List<Dictionary<string, object>> data = model.Get(id);
    }
    private void LoadToVars()
    {
        
    }
}
