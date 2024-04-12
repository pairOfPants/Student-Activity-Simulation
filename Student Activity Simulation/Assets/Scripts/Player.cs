using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string FirstName;
    public string LastName;
    public int Age;
    public string Address;

    public Player(string fName, string lName, int age, string address)
    {
        FirstName = fName;
        LastName = lName;
        Age = age;
        Address = address;
    }

    public Player(string fName, string lName, int age)
    {
        FirstName = fName;
        LastName = lName;
        Age = age;
        Address = "";
    }

    public string GetFirstName() {return FirstName;}
    public string GetLastName() {return LastName;}
    public void SetFirstName(string name) {FirstName = name;}
    public void SetLastName(string name) {LastName = name;}
    public int GetAge(){return Age;}
    public void SetAge(int age) {Age = age;}
    public string GetAddress(){return Address;}
    public void SetAddress(string address){Address = address;}
}
