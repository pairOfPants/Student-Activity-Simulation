using UnityEngine;
using System.Collections;
public class Controller : MonoBehaviour 
{
    //variables declared for simple movement of camera
    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Difference;
    private bool Drag=false;
    [SerializeField]
    private Camera mainCam;
    private Vector3 WorldCoordClickDist; //World Position of Mouse Input


    //variables needed to zoom camera in and out
    private float zoom;
    private float zoomMultiplier = 2500f;
    private float minZoom = -400f;
    private float maxZoom = 5500f;
    private float scrollVelocity = 0f;
    private float smoothTime = 0.0125f;



    //variables for making sure camera doesn't go out of bounds
    [SerializeField]
    private float xMax = 415;
    [SerializeField]
    private float xMin = -1325;
    [SerializeField]
    private float yMax = 735;
    [SerializeField]
    private float yMin = -236;

    private float originalAspect;
    void Start () 
    {
        zoom = mainCam.orthographicSize;
        ResetCamera = mainCam.transform.position; //calculates center of camera at program start
        originalAspect = mainCam.aspect;
    }

    void Update()
    {

        if(Input.GetAxis("Mouse ScrollWheel")!= 0)
        {
            xMax = 415;
            xMin = -1325;
            yMax = 735;
            yMin = -236;
            float scroll = Input.GetAxis("Mouse ScrollWheel"); //0.1 for zooming in, -0.1 for out
            //Debug.Log(scroll);
            zoom -= scroll * zoomMultiplier;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            mainCam.orthographicSize = Mathf.SmoothDamp(mainCam.fieldOfView, zoom, ref scrollVelocity, smoothTime);
            mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, 100, 250);

            float height = mainCam.orthographicSize; //calculates height of camera using zoom
            float width = height * mainCam.aspect; //calculates width of camera using zoom

            xMin = xMin / (mainCam.aspect/originalAspect);
            xMax = xMax / (mainCam.aspect/originalAspect);
            yMin = yMin / (mainCam.orthographicSize/100);
            yMax = yMax / (mainCam.orthographicSize/100);
            
        }
    }
    void LateUpdate () 
    {
        if (Input.GetMouseButton (0)) 
        {
            //calculates the world coordinates of the mouse position at time of click
            WorldCoordClickDist = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.transform.position.z * -1));
            Difference= WorldCoordClickDist- mainCam.transform.position; //The difference between middle of camera and where we clicked
            if (Drag==false)
            {
                Drag=true;
                Origin=WorldCoordClickDist; //middle of camera at time of click
            }
        } 
        else 
        {
            Drag=false;
        }
        if (Drag==true) //if user is moving camera
        {
            mainCam.transform.position = Origin-Difference; //Updates Camera Position according to calculations made above

            // var matrix =  mainCam.cameraToWorldMatrix; //matrix that stores camera's viewing pixels as a 2d array
            // var bottomLeft = matrix.MultiplyPoint(new Vector3(mainCam.pixelRect.xMin, mainCam.pixelRect.yMin)); //bottom Left Coordinate of viewing distance
            // var upperLeft = matrix.MultiplyPoint(new Vector3(mainCam.pixelRect.xMin, mainCam.pixelRect.yMax)); //Upper Left Coordinate
            // var upperRight = matrix.MultiplyPoint(new Vector3(mainCam.pixelRect.xMax, mainCam.pixelRect.yMax)); //Upper Right Coordinate
            
            
            // var rect = new Rect(upperLeft.x, upperLeft.y, width, height); //Rectangle Object that represents what the camera sees
            // if(rect.xMin <= xMin) 
            // {
            //     Debug.Log("LEFT EDGE REACHED");
            //     mainCam.transform.position = new Vector3(xMin,mainCam.transform.position.y,mainCam.transform.position.z);
            // }
            // if(rect.yMin <= yMin) 
            // {
            //     Debug.Log("BOTTOM EDGE REACHED");
            //     //mainCam.transform.position = new Vector3(rect.center.x,rect.center.y,mainCam.transform.position.z);
            // }
            // if(rect.xMax >= xMax) 
            // {
            //     Debug.Log("RIGHT EDGE REACHED");
            //     //mainCam.transform.position = new Vector3(rect.center.x,rect.center.y,mainCam.transform.position.z);
            // }
            // if(rect.yMax >= yMax) 
            // {
            //     Debug.Log("TOP EDGE REACHED");
            //     //mainCam.transform.position = new Vector3(rect.center.x,rect.center.y,mainCam.transform.position.z);
            // }

            if(mainCam.transform.position.x < xMin) mainCam.transform.position = new Vector3(xMin,mainCam.transform.position.y,mainCam.transform.position.z);
            if(mainCam.transform.position.x > xMax) mainCam.transform.position = new Vector3(xMax,mainCam.transform.position.y,mainCam.transform.position.z);
            if(mainCam.transform.position.y < yMin) mainCam.transform.position = new Vector3(mainCam.transform.position.x,yMin,mainCam.transform.position.z);
            if(mainCam.transform.position.y > yMax) mainCam.transform.position = new Vector3(mainCam.transform.position.x,yMax,mainCam.transform.position.z);
         }
        //RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
        if (Input.GetMouseButton (1)) 
        {
            mainCam.transform.position=ResetCamera;
        }


        
        
        
        
    }
} 