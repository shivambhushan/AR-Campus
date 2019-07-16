using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EVENT : MonoBehaviour {

    private int EventID;
    private string Header;
    private int PersonID;
    private DateTime Date;

    public EVENT()
    {
      
    }

    public EVENT(int eventID, string header, int personID, DateTime date)
    {
        EventID = eventID;
        Header = header;
        PersonID = personID;
        Date = date;
    }

    public int EventID1
    {
        get
        {
            return EventID;
        }

        set
        {
            EventID = value;
        }
    }

    public string Header1
    {
        get
        {
            return Header;
        }

        set
        {
            Header = value;
        }
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

    public DateTime Date1
    {
        get
        {
            return Date;
        }

        set
        {
            Date = value;
        }
    }

    public string generateEventQuery(int personID)
    {
        string sqlQuery;
        sqlQuery = "SELECT *FROM EVENT WHERE PersonID=" + "'" + personID + "'";
        return sqlQuery;
    }
}
