using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Driver : MonoBehaviour
{
     public GameObject prefab;
    private Pathfinding pathfinding;
    private Grid<PathNode> grid;

    [SerializeField] private GameObject mapOfUMBC;
    private int pathFindingGridCellSize = 10; 

    // Start is called before the first frame update
    void Start()
    {
        //Logic to get corners of map to load into grid
        RectTransform rect = mapOfUMBC.GetComponent<RectTransform>();
        Vector3[] mapCorners = new Vector3[4];
        rect.GetWorldCorners(mapCorners);
        int mapWidth = (int)mapCorners[2].x - (int)mapCorners[0].x; //right x minus left x
        int mapHeight = (int)mapCorners[2].y - (int)mapCorners[0].y; //top y minus bottom y

        mapWidth = mapWidth / pathFindingGridCellSize; //adjusts coordinates for width and height
        mapHeight = mapHeight / pathFindingGridCellSize;

        Vector3 mapBottomLeft = mapCorners[0];


        pathfinding = new Pathfinding(mapWidth,mapHeight,pathFindingGridCellSize, mapBottomLeft); //Assigns pathfinding object to top right of map, spanning the whole map
        grid = pathfinding.GetGrid();

        updateGridForNonWalkables();

        //Student test = new Student("Aidan", "Denham", 19, "Potomac Hall", true, 73);
        Place residence = new Place("Potomac Hall");
        Place destination = new Place("True Grits");
        Vector3 from = GameObject.Find(residence.ToString()).GetComponent<Transform>().position;
        Vector3 to = GameObject.Find(destination.ToString()).GetComponent<Transform>().position;
        List<Vector3> path = new List<Vector3>();
        path = pathfinding.FindPath(from,to);
        // Debug.Log(path.Count);
        // foreach(Vector3 p in path)
        // {
        //     Debug.Log(p);
        // }

        GameObject student = Instantiate(prefab, path[0], Quaternion.identity);
        student.GetComponent<Student>().FirstName = "Aidan";
        student.GetComponent<Student>().LastName = "Denham";
        student.GetComponent<Student>().Age = 19;
        student.GetComponent<Student>().Address = "Potomac Hall";
        student.GetComponent<Student>().isCommuter = false;
        student.GetComponent<Student>().height = 71;
        student.GetComponent<Student>().path = path;
        student.GetComponent<Student>().walkingSpeed = student.GetComponent<Student>().calculateWalkSpeed();
        student.GetComponent<Student>().stepsToGo = path.Count;
        //test.AddComponent<Student>("Aidan", "Denham", 19, "Potomac Hall", true, 73);

        //test.DrawToScreen(path[0]);


    }

    // Update is called once per frame
    void Update()
    {

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
}
