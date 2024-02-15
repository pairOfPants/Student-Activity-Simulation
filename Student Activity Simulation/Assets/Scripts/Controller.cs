using UnityEngine;
using System.Collections;
public class Controller : MonoBehaviour 
{
    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Difference;
    private bool Drag=false;
    [SerializeField]
    private Camera mainCam;
    private Vector3 WorldCoordClickDist; //World Position of Mouse Input

    private float zoom;
    private float zoomMultiplier = 2500f;
    private float minZoom = -400f;
    private float maxZoom = 5500f;
    private float scrollVelocity = 0f;
    private float smoothTime = 0.0125f;
    void Start () 
    {
        zoom = mainCam.orthographicSize;
        ResetCamera = mainCam.transform.position; //calculates center of camera at program start
    }

    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel")!= 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel"); //0.1 for zooming in, -0.1 for out
            Debug.Log(scroll);
            zoom -= scroll * zoomMultiplier;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            mainCam.orthographicSize = Mathf.SmoothDamp(mainCam.fieldOfView, zoom, ref scrollVelocity, smoothTime);
            mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize, 100, 550);
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
        if (Drag==true)
        {
            mainCam.transform.position = Origin-Difference; //Updates Camera Position according to calculations made above
        }
        //RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
        if (Input.GetMouseButton (1)) 
        {
            mainCam.transform.position=ResetCamera;
        }
        
    }
} 