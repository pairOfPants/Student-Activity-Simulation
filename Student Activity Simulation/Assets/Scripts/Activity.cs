using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Activity : MonoBehaviour
{ 
    public string Name; //Name of activity
    public customTime StartTime; //Start time in hours,minutes format
    public customTime EndTime; //End time in hours,minutes format
    public int NumOfDays; //Number of days during the week this event occurs (clamped between 0 and 7)
    public bool[] Weekdays = {false,false,false,false,false,false,false}; //does this event happen yes or no for each day of the week
    public Place PlaceOfOccurrence; //place on the map this event occurs
    public int Priority; //if two events overlap, schedule will choose event with higher priority
    public int NumOfTimesCompleted; //if two events overlap with same priority, schedule will do the event done less times


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
