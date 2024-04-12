using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{
    public const double OFF_CAMPUS_RESIDENCE_COORDINATE = 20000;
    public string Name;
    public double XStart;
    public double XEnd;
    public double YStart;
    public double YEnd;

    //public GameObject locationOnMap;
    
    public Place(string name) //for commuters
    {
        this.Name = name;
        XStart = OFF_CAMPUS_RESIDENCE_COORDINATE;
        XEnd = OFF_CAMPUS_RESIDENCE_COORDINATE;
        YStart = OFF_CAMPUS_RESIDENCE_COORDINATE;
        YEnd = OFF_CAMPUS_RESIDENCE_COORDINATE;
        //locationOnMap = null;
    }

    public Place(string name, int XStart, int xEnd, int YStart, int YEnd)
    {
        this.Name = name;
        this.XStart = XStart;
        this.XEnd = xEnd;
        this.YStart = YStart;
        this.YEnd = YEnd;
    }

    public override string ToString()
    {
        return Name;
    }
}
