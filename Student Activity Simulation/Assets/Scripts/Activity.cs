using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour
{ 
    protected string Name; //Name of activity
    protected System.DateTime StartTime; //Start time in hours,seconds format
    protected System.DateTime EndTime; //End time in hours,seconds format
    protected int NumOfDays; //Number of days during the week this event occurs (clamped between 0 and 7)
    protected bool[] Weekdays = {false,false,false,false,false,false,false}; //does this event happen yes or no for each day of the week
    protected Place PlaceOfOccurrence; //place on the map this event occurs
    protected int Priority; //if two events overlap, schedule will choose event with higher priority
    protected int NumOfTimesCompleted; //if two events overlap with same priority, schedule will do the event done less times


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
