using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Driver : MonoBehaviour
{
    public GameObject prefab;
    private Student studentClass;
    private Pathfinding pathfinding;
    private Grid<PathNode> grid;

    private List<Student> totalStudents;

    private List<Place> totalPlaces;

    [SerializeField] private GameObject mapOfUMBC;
    private int pathFindingGridCellSize = 10; 

    //CLOCK VARIABLES
    private float startTime;
    private float elapsedTime;
    GameObject gameTime;

    //UI ELEMENTS
    public GameObject playerStatsUI;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        //TIME LOGIC
        System.DateTime startDateTime = System.DateTime.Now; //time and date of program start
        gameTime = new GameObject("TIMER FOR GAME");
        gameTime.AddComponent<customTime>();
        gameTime.GetComponent<customTime>().hour = startDateTime.Hour;
        gameTime.GetComponent<customTime>().minute = startDateTime.Minute;
        gameTime.GetComponent<customTime>().second = startDateTime.Second;
        System.DayOfWeek dayOfWeek = startDateTime.DayOfWeek;
        if(dayOfWeek == System.DayOfWeek.Sunday) gameTime.GetComponent<customTime>().day = 0;
        else if(dayOfWeek == System.DayOfWeek.Monday) gameTime.GetComponent<customTime>().day = 1;
        else if(dayOfWeek == System.DayOfWeek.Tuesday) gameTime.GetComponent<customTime>().day = 2;
        else if(dayOfWeek == System.DayOfWeek.Wednesday) gameTime.GetComponent<customTime>().day = 3;
        else if(dayOfWeek == System.DayOfWeek.Thursday) gameTime.GetComponent<customTime>().day = 4;
        else if(dayOfWeek == System.DayOfWeek.Friday) gameTime.GetComponent<customTime>().day = 5;
        else if(dayOfWeek == System.DayOfWeek.Saturday) gameTime.GetComponent<customTime>().day = 6;
        startTime = Time.time;

        //INITIALIZE ALL VARIABLES
        studentClass = prefab.GetComponent<Student>();
        totalPlaces = new List<Place>();
        totalStudents = new List<Student>();
        playerStatsUI.SetActive(false);

        //Logic to get corners of map to load into grid
        RectTransform rect = mapOfUMBC.GetComponent<RectTransform>();
        Vector3[] mapCorners = new Vector3[4];
        rect.GetWorldCorners(mapCorners);
        int mapWidth = (int)mapCorners[2].x - (int)mapCorners[0].x; //right x minus left x
        int mapHeight = (int)mapCorners[2].y - (int)mapCorners[0].y; //top y minus bottom y
        mapWidth = mapWidth / pathFindingGridCellSize; //adjusts coordinates for width and height
        mapHeight = mapHeight / pathFindingGridCellSize;
        
        
        //GRID LOAD LOGIC
        Vector3 mapBottomLeft = mapCorners[0];
        pathfinding = new Pathfinding(mapWidth,mapHeight,pathFindingGridCellSize, mapBottomLeft); //Assigns pathfinding object to top right of map, spanning the whole map
        grid = pathfinding.GetGrid();
        updateGridForNonWalkables();

        loadPlaces(); //loads all places into list



        //STUDENT LOGIC HERE
        totalStudents.Add(InstantiateStudent("Aidan", "Denham", 19, "Potomac Hall", false, 73));
        GeneratePath(totalStudents[0],findPlace("True Grits"));



    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = Time.time - startTime;
        if(gameTime.GetComponent<customTime>().ToString() == "0:00") //at midnight every night will do most intensive calculations
        {
            foreach(Student s in totalStudents)
            {
                s.GetComponent<Student>().dailySchedule = s.GenerateSchedule(gameTime.GetComponent<customTime>().day);
            }
        }

        foreach(Student s in totalStudents)
        {
            if(s.needToMove)
            {
                Move(s, s.GetComponent<Student>().path, s.GetComponent<Student>().currentWorldPosition);
            }
            if(Input.GetMouseButtonDown(0))
            {
                if(approximatelyEqual(s.GetComponent<Student>().currentWorldPosition, cam.ScreenToWorldPoint(Input.mousePosition)))
                {
                    playerStatsUI.SetActive(true);
                    TMP_InputField[] textFields = playerStatsUI.GetComponents<TMP_InputField>();
                    Debug.Log(textFields.Length);
                    foreach(TMP_InputField text in textFields)
                    {
                        if(text.name == "First Name Input")
                        {
                            Debug.Log("FIRST NAME CHANGED");
                            text.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshPro>().text = s.GetComponent<Student>().FirstName;
                        }
                        if(text.name == "Last Name Input")
                        {
                            Debug.Log("LAST NAME CHANGED");
                            text.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshPro>().text = s.GetComponent<Student>().LastName;
                        }
                    }
                }
                else playerStatsUI.SetActive(false);
            }
        }
    }

    private void updateGridForNonWalkables()
    {
        GameObject[] nonWalkables = GameObject.FindGameObjectsWithTag("NonWalkable");

        for(int x = 0; x<grid.GetWidth(); x++)
        {
            for(int y = 0; y<grid.GetHeight(); y++) //outer for loops to loop through each grid object
            {
                if(grid.GetGridObject(x,y).isWalkable)
                {
                    for(int i = 0; i<nonWalkables.Length; i++) //inner for loop to loop through each nonwalkable
                    {
                        Transform transform = nonWalkables[i].GetComponent<Transform>();
                        if(Vector3.Distance(transform.position,grid.GetWorldPosition(x,y)) < 0.01) grid.GetGridObject(x,y).SetIsWalkable(false);
                    }
                } 
            }
        }
    }

    private Student InstantiateStudent(string fname, string lname, int age, string address, bool isCommuter, int height)
    {
        Place residence = new Place("Potomac Hall");
        Place start = findPlace(address);
        Student student = Instantiate(studentClass, start.center, Quaternion.identity);
        student.GetComponent<Student>().FirstName = fname;
        student.GetComponent<Student>().LastName = lname;
        student.GetComponent<Student>().Age = age;
        student.GetComponent<Student>().Address = address;
        student.GetComponent<Student>().residence = start;
        student.GetComponent<Student>().isCommuter = isCommuter;
        student.GetComponent<Student>().height = height;
        student.GetComponent<Student>().walkingSpeed = student.GetComponent<Student>().calculateWalkSpeed();
        student.GetComponent<Student>().currentWorldPosition = start.center;

        return student;

    }

    public void Move(Student student, List<Vector3> path, Vector3 currentPosition)
    {
        Transform transform = student.GetComponent<Transform>();
        if(path.Count > 0)
        {
            try
            {
                while(path[0].x == currentPosition.x && path.Count > 0) path.RemoveAt(0);
                while(path[0].y == currentPosition.y && path.Count > 0) path.RemoveAt(0);

                float xDirection = (path[0].x-currentPosition.x)/Mathf.Abs(path[0].x-currentPosition.x);
                float yDirection = (path[0].y-currentPosition.y)/Mathf.Abs(path[0].y-currentPosition.y);

            student.transform.position = Vector3.Lerp(currentPosition, new Vector3(currentPosition.x + (5*xDirection), currentPosition.y + (5*yDirection)), Time.deltaTime*student.GetComponent<Student>().walkingSpeed);
            student.GetComponent<Student>().currentWorldPosition = student.transform.position;
            }
            catch (System.Exception)
            {
                
            }
            
        }
        
        
        student.GetComponent<Student>().currentWorldPosition = student.transform.position;

        if(path.Count == 0) //student should be on a corner of the place's rectangle now
        {
            if(student.transform.position.x == student.GetComponent<Student>().destinationPlace.XStart || 
               student.transform.position.x == student.GetComponent<Student>().destinationPlace.XEnd ||
               student.transform.position.y == student.GetComponent<Student>().destinationPlace.YStart ||
               student.transform.position.y == student.GetComponent<Student>().destinationPlace.YEnd)
               {
                    student.GetComponent<Student>().currentPlace = student.GetComponent<Student>().destinationPlace;
                    student.transform.position = student.GetComponent<Student>().destinationPlace.center;
                    student.GetComponent<Student>().needToMove = false;
                    student.currentPlace = student.destinationPlace;
                    student.transform.position = student.destinationPlace.center;
                    student.needToMove = false;
                    student.GetComponent<Student>().currentWorldPosition = student.transform.position;
                    //continue;
               }
        }
        
        // if(path.Count > 1)transform.position = Vector3.Lerp(path[0], path[1], Time.deltaTime * percentThere);
        // else transform.position = Vector3.Lerp(transform.position, path[0], Time.deltaTime * percentThere* walkingSpeed);
    }

    public List<Vector3> GeneratePath(Student student, Place destination)
    {
        List<Vector3> path = new List<Vector3>();
        path = pathfinding.FindPath(student.currentWorldPosition,destination.center);

        student.GetComponent<Student>().destinationPlace = destination;
        student.GetComponent<Student>().path = path;

        if(destination.center != student.GetComponent<Student>().currentWorldPosition) student.GetComponent<Student>().needToMove = true;
        else student.GetComponent<Student>().needToMove = false;

        return path;
    }

    private void loadPlaces()
    {
        GameObject[] placeObjects = GameObject.FindGameObjectsWithTag("NonWalkable");
        foreach(GameObject obj in placeObjects)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            Vector3 topRight = rend.bounds.max;
            Vector3 bottomLeft = rend.bounds.min;

            Place currentPlace = new Place(obj.name, bottomLeft.x, topRight.x, bottomLeft.y, topRight.y);
            totalPlaces.Add(currentPlace);
        }
    }

    private Place findPlace(string name)
    {
        foreach(Place p in totalPlaces)
        {
            if(p.Name == name) return p;
        }
        return new Place(name);
    }

    private bool approximatelyEqual(Vector3 v1, Vector3 v2)
    {
        int misclickRadius = 5;

        if(Mathf.Abs(v1.x-v2.x) <= misclickRadius && Mathf.Abs(v1.y-v2.y) <= misclickRadius) return true;
        else return false;
    }

}
