using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Player
{
    public List<Activity> dailySchedule;
    public List<Activity> totalActivities;
    public Place residence;
    public bool isCommuter;
    public int height;
    public float walkingSpeed;

    public List<Vector3> path;
    private bool needToMove;

    public int stepsMoved = 0;
    public int stepsToGo = 0;
    
    public Student(string fName, string lName, int age, string address, bool isCommuter, int height) : base(fName, lName, age, address)
    {
        this.isCommuter = isCommuter;
        this.height = height;
        if(!isCommuter) 
        {
            GameObject temp = GameObject.Find(address);
            //residence =  new Place(address, temp.position.)
            //NEED to find a way to take game object and find its corners
        }
        else residence = new Place(address);

        dailySchedule = new List<Activity>();
        totalActivities = new List<Activity>();
        walkingSpeed = calculateWalkSpeed();
    }

    void Update()
    {
        if(path != null && path.Count > 0) needToMove = true;
        else needToMove = false;

        if(needToMove)
        {
            stepsMoved++;
            Move(path, (float)stepsMoved/stepsToGo);
            path.Remove(path[0]);
        }
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
                            if(i.NumOfTimesCompleted > j.NumOfTimesCompleted) todayEvents.Remove(i);
                            else if(j.NumOfTimesCompleted > i.NumOfTimesCompleted) todayEvents.Remove(j);
                            else //case same priority and both have been completed same number of times
                            {
                                //REMOVE BY NUMBER OF TIMES THIS ACTIVITY IS DURING THE WEEK
                                if(i.NumOfDays > j.NumOfTimesCompleted) todayEvents.Remove(i);
                                else todayEvents.Remove(j); //removes j even if completed the same amount of times because the chances of this occurring are very low
                            }
                        }
                    }
                }
            }
        }
        

        return todayEvents;
    }
    public float calculateDistance(Place to, Place from)
    {
        return 0;
    }

    public float calculateWalkSpeed()
    { //code implementation of model described HERE: https://jms.ump.edu.pl/index.php/JMS/article/view/817/988
        float strideFrequency; 
        float strideLength;
        float walkingSpeed;

        strideFrequency = 2f*Mathf.PI*Mathf.Sqrt(height/9.8f);
        strideLength = 1.7f*strideFrequency - 0.3f;

        walkingSpeed = strideLength / strideFrequency;

        return walkingSpeed;
    }

    public float calculateWalkTime(Vector3 start, Vector3 end)
    {
        float xDist = end.x-start.x;
        float yDist = end.y-start.y;
        float distance = Mathf.Sqrt((xDist*xDist)-(yDist*yDist));
        return distance/walkingSpeed;
    }

    public void Move(List<Vector3> path, float percentThere)
    {
        Transform transform = this.GetComponent<Transform>();
        if(path.Count > 1)transform.position = Vector3.Lerp(path[0], path[1], Time.deltaTime * percentThere*walkingSpeed * 100000000000);
        else transform.position = Vector3.Lerp(transform.position, path[0], Time.deltaTime * percentThere* walkingSpeed * 100000000000);

    }

}
