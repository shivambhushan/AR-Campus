using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PERSON : MonoBehaviour
{
    private int PersonID;
    private string Name;
    private string Surname;
    private string OutlookMail;
    private string Password;
    private string Title;
    private string Department;
    private string Team;
    private string Speciality;
    private string PersonalInfo;
    private string ARFotoName;

    public PERSON()
    {
    }

    public PERSON(int personID1, string name1, string surname1, string outlookMail1, string password1, string title1, string department1, string team1, string speciality1, string personalInfo1, string aRFotoName1)
    {
        PersonID1 = personID1;
        Name1 = name1;
        Surname1 = surname1;
        OutlookMail1 = outlookMail1;
        Password1 = password1;
        Title1 = title1;
        Department1 = department1;
        Team1 = team1;
        Speciality1 = speciality1;
        PersonalInfo1 = personalInfo1;
        ARFotoName1 = aRFotoName1;
    }

    public int PersonID1
    {
        get
        {
            return PersonID;
        }

        set
        {
            PersonID = value;
        }
    }

    public string Name1
    {
        get
        {
            return Name;
        }

        set
        {
            Name = value;
        }
    }

    public string Surname1
    {
        get
        {
            return Surname;
        }

        set
        {
            Surname = value;
        }
    }

    public string OutlookMail1
    {
        get
        {
            return OutlookMail;
        }

        set
        {
            OutlookMail = value;
        }
    }

    public string Password1
    {
        get
        {
            return Password;
        }

        set
        {
            Password = value;
        }
    }

    public string Title1
    {
        get
        {
            return Title;
        }

        set
        {
            Title = value;
        }
    }

    public string Department1
    {
        get
        {
            return Department;
        }

        set
        {
            Department = value;
        }
    }

    public string Team1
    {
        get
        {
            return Team;
        }

        set
        {
            Team = value;
        }
    }

    public string Speciality1
    {
        get
        {
            return Speciality;
        }

        set
        {
            Speciality = value;
        }
    }

    public string PersonalInfo1
    {
        get
        {
            return PersonalInfo;
        }

        set
        {
            PersonalInfo = value;
        }
    }

    public string ARFotoName1
    {
        get
        {
            return ARFotoName;
        }

        set
        {
            ARFotoName = value;
        }
    }

    public string generatePersonQuery(string uniqueFotoName)
    {
        string sqlQuery;
        sqlQuery = "SELECT *FROM PERSON WHERE ARFotoName=" + "'" + uniqueFotoName + "'";
        return sqlQuery;
    }

    public string generatePersonQuery(string username, string password)
    {
        string sqlQuery;
        sqlQuery = "SELECT *FROM PERSON WHERE OutlookMail=" + "'" + username + "' AND Password= '" + password + "'";
        return sqlQuery;
    }
}
