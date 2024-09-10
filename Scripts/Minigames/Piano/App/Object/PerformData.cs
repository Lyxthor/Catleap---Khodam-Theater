using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class TimeLanePair
{
    public float time;
    public int lane;
}
[CreateAssetMenu]
public class PerformData : ScriptableObject
{
    public int auraPrize;
    public VideoClip videoClip;
    public TimeLanePair[] timeLanePair;
}
