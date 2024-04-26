using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{
    public const float OFF_CAMPUS_RESIDENCE_COORDINATE = 20000f;
    public string Name;
    public float XStart;
    public float XEnd;
    public float YStart;
    public float YEnd;
    
    public Vector3 center;

    //public GameObject locationOnMap;
    
    public Place(string name) //for commuters
    {
        this.Name = name;
        XStart = OFF_CAMPUS_RESIDENCE_COORDINATE;
        XEnd = OFF_CAMPUS_RESIDENCE_COORDINATE;
        YStart = OFF_CAMPUS_RESIDENCE_COORDINATE;
        YEnd = OFF_CAMPUS_RESIDENCE_COORDINATE;
        center = new Vector3((XStart+XEnd)/2, (YStart+YEnd)/2);
        //locationOnMap = null;
    }

    public Place(string name, float XStart, float xEnd, float YStart, float YEnd)
    {
        this.Name = name;
        this.XStart = XStart;
        this.XEnd = xEnd;
        this.YStart = YStart;
        this.YEnd = YEnd;
        center = new Vector3((XStart+XEnd)/2, (YStart+YEnd)/2);
    }

    public override string ToString()
    {
        return Name;
    }
}
