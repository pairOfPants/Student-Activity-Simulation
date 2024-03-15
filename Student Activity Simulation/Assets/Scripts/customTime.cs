using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class customTime : MonoBehaviour
{
    [SerializeField]
    protected int hour;
    [SerializeField]
    protected int minute;
    // Start is called before the first frame update
    public customTime(int h, int m)
    {
        hour = Mathf.Clamp(h, 1, 12);;
        minute = Mathf.Clamp(m, 1, 12);;
    }
    public string toString()
    {
        string ret = (hour + ":" + minute);
        return ret;
    }
}
