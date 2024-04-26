using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class customTime : MonoBehaviour
{
    [SerializeField]
    public int hour;
    [SerializeField]
    public int minute;

    public int second;
    private float startTime;

    public int day;

    private float timer = 0.0f;

    //private Stopwatch stopwatch;
    // Start is called before the first frame update
    public customTime(int h, int m, int s, int d)
    {
        hour = Mathf.Clamp(h, 1, 24);
        minute = Mathf.Clamp(m, 0, 59);
        second = Mathf.Clamp(s,0,59);
        day = Mathf.Clamp(d, 0, 6);
    }

    void Start()
    {
        startTime = Time.time;
        // stopwatch = new Stopwatch();
        // stopwatch.Start();
    }

    void Update()
    {
        if((timer += Time.deltaTime) >= 1.0f)
        {
            second++;
            timer-=1;
        }
        if(second >= 60)
        {
            second = 0;
            minute++;
        }
        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }
        if(hour >= 24)
        {
            hour = 0;
            day++;
        }
        if(day >= 7) day = 0;
    }
    public string toString()
    {
        string ret = (hour + ":" + minute);
        return ret;
    }

}
