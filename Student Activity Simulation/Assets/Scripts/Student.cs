using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Player
{
    protected List<Activity> dailySchedule;
    protected List<Activity> totalActivities;
    protected Place residence;
    protected bool isCommuter;
    protected int height;
    protected int walkingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        dailySchedule = new List<Activity>();
        totalActivities = new List<Activity>();

        walkingSpeed = calculateWalkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Activity> GenerateSchedule(int dayOfWeek)
    {
        List<Activity> todayEvents = new List<Activity>();
        foreach(Activity i in totalActivities) //adds all daily activities to list
        {
            if(i.Weekdays[dayOfWeek] == true)
            todayEvents.Add(i);
        }

        //sorts the list on startTime / priority
        foreach(Activity i in todayEvents) //adds all daily activities to list
        {
            foreach(Activity j in todayEvents)
            {
                if(i != j)
                {
                    if(i.StartTime == j.StartTime)
                    {
                        if(i.Priority < j.Priority) todayEvents.Remove(i);
                        else if(j.Priority < i.Priority) todayEvents.Remove(j);
                        else //case of two same-priority events
                        {
                            //REMOVE BY NUM OF TIMES COMPLETED
                            if(i.NumOfTimesCompleted < j.NumOfTimesCompleted) todayEvents.Remove(i);
                            else if(j.NumOfTimesCompleted < i.NumOfTimesCompleted) todayEvents.Remove(j);
                            else //case same priority and both have been completed same number of times
                            {
                                //REMOVE BY NUMBER OF TIMES THIS ACTIVITY IS DURING THE WEEK
                                if(i.NumOfDays < j.NumOfTimesCompleted) todayEvents.Remove(i);
                                else todayEvents.Remove(j); //removes j even if completed the same amount of times because the chances of this occurring are very low
                            }
                        }
                    }
                }
            }
        }
        //

        return todayEvents;
    }
    public int calculateDistance(Place to, Place from)
    {
        return 0;
    }

    public float calculateWalkSpeed()
    { //code implementation of model described HERE: https://jms.ump.edu.pl/index.php/JMS/article/view/817/988
        float strideFrequency;
        float strideLength;
        float walkingSpeed;

        strideFrequency = 2*Mathf.PI*Mathf.sqrt(height/9.8);
        strideLength = 1.7f*strideFrequency - 0.3f;

        walkingSpeed = strideLength / strideFrequency;

        return walkingSpeed;
    }

    public float calculateWalkTime(float distance)
    {
        return distance/walkingSpeed;
    }
}
