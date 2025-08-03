using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    float startTime;
    float duration;
    public Timer(float duration)
    {
        startTime = Time.time;
        this.duration = duration;
    }

    public Timer(float startTime , float duration)
    {
        this.startTime = startTime;
        this.duration = duration;
    }

    public bool IsFinished() => startTime + duration <= Time.time;

    public float GetRemainingSecs()
    {
        float rSecs = startTime + duration - Time.time;
        return (rSecs < 0f) ? 0f : rSecs;
    }




}
