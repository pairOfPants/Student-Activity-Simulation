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
    //MOVING VARIABLES
    public bool needToMove;

    public int stepsMoved = 0;
    public int stepsToGo = 0;
    public Vector3 currentWorldPosition;
    public Place currentPlace; //where student currently is - updated after every completed move
    public Place destinationPlace; //where student is trying to go - will be null if nowhere to go
    
    public static Student Instance { get; private set; }
    
    public Student(string fName, string lName, int age, string address, bool isCommuter, int height) : base(fName, lName, age, address)
    {
        Instance = this;
        this.isCommuter = isCommuter;
        this.height = height;
        // if(!isCommuter) 
        // {
        //     GameObject temp = GameObject.Find(address);

        //     //residence =  new Place(address, temp.position.)
        //     //NEED to find a way to take game object and find its corners
        // }
        // else residence = new Place(address);


        dailySchedule = new List<Activity>();
        totalActivities = new List<Activity>();
        walkingSpeed = calculateWalkSpeed();

        currentWorldPosition = residence.center;
        needToMove = false;
        
    }

    void Update()
    {
        // if(currentWorldPosition != destinationPlace.center) needToMove = true;
        // else needToMove = false;


        // if(needToMove)
        // {
        //     Move(path, currentWorldPosition);
        // }
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

    public void InsertActivity(Activity temp, int currentDayOfWeek) //MUST BE 0-6
    {
        totalActivities.Add(temp);
        if(temp.Weekdays[currentDayOfWeek]) //if the activity happens today regenerate daily schedule
        {
            GenerateSchedule(currentDayOfWeek);
        }
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

    public void Move(List<Vector3> path, Vector3 currentPosition)
    {
        Transform transform = this.GetComponent<Transform>();
        if(currentPosition == path[0]) path.RemoveAt(0);
        
        float xDirection = (path[0].x-currentPosition.x)/Mathf.Abs(path[0].x-currentPosition.x);
        float yDirection = (path[0].y-currentPosition.y)/Mathf.Abs(path[0].y-currentPosition.y);
        
        if(path.Count > 0) transform.position  = Vector3.Lerp(currentPosition, new Vector3(currentPosition.x + (5*xDirection), currentPosition.y + (5*yDirection)), Time.deltaTime);
        
        // if(path.Count > 1)transform.position = Vector3.Lerp(path[0], path[1], Time.deltaTime * percentThere);
        // else transform.position = Vector3.Lerp(transform.position, path[0], Time.deltaTime * percentThere* walkingSpeed);
    }


}
