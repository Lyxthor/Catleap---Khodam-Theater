using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private TimerView view;
    public int interval;
    private int current;
    private Action action;
    private Coroutine countDown;
    private bool paused=false;
    private void Awake()
    {
        view = GetComponent<TimerView>();
        
    }
    private void Start()
    {
        if (interval > 0 && countDown==null && !paused) countDown = StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        current = current > 0 ? current : interval;
        while(current >= 0)
        {
            yield return new WaitForSeconds(1);
            view.SetTimerText(current);
            current--;
            if (current+1 == 0)
            {
                if (action == null) DefaultTest();
                else action();
                current = interval;
            }
        }
    }
    public void TogglePauseCountDown()
    {
        paused = !paused; 
        if (!paused) countDown = StartCoroutine(CountDown());
        else if(countDown!=null) StopCoroutine(countDown);
     
    }
    public void SetPaused(bool _state)
    {
        paused = _state;
        if (_state)
        {
            StopAllCoroutines();
            countDown = null;
        }
        else if (countDown == null)
        {
            countDown = StartCoroutine(CountDown());
        }
    }
    public int GetCurrent()
    {
        return current;
    }
    public int GetInterval()
    {
        return interval;
    }
    public static IEnumerator SetTimeout(int _timeout, Action _action)
    {
        yield return new WaitForSeconds(_timeout);
        _action();
    }
    public static IEnumerator SetInterval(int _interval, Action _action)
    {
        int sekon = _interval;
        while(sekon>=0)
        {
            yield return new WaitForSeconds(1);
            sekon--;
            if (sekon + 1 == 0)
            {
                _action();
                sekon = _interval;
            }
        }
    }
    public static IEnumerator SetPreciseInterval(float _interval, Action _action)
    {
        float sekon = _interval;
        while (sekon >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            sekon -= 0.1f;
            if(sekon < 0)
            {
                _action();
                sekon = _interval;
            }
        }
    }
    private void DefaultTest()
    {
        Debug.Log("Default Action");
    }
    public void SetData(int _interval, Action _action=null)
    {
        interval = _interval;
        action = _action == null ? DefaultTest : _action ;
        
    }
}
