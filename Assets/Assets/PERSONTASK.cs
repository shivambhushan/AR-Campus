using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PERSONTASK : MonoBehaviour
{

    private int TaskID;
    private int PersonID;
    private string TaskName;
    private int ProjectID;

    public PERSONTASK()
    {
    }

    public PERSONTASK(int taskID, int personID, string taskName, int projectID)
    {
        TaskID1 = taskID;
        PersonID1 = personID;
        TaskName1 = taskName;
        ProjectID1 = projectID;
    }

    public int TaskID1
    {
        get
        {
            return TaskID;
        }

        set
        {
            TaskID = value;
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

    public string TaskName1
    {
        get
        {
            return TaskName;
        }

        set
        {
            TaskName = value;
        }
    }

    public int ProjectID1
    {
        get
        {
            return ProjectID;
        }

        set
        {
            ProjectID = value;
        }
    }

    public string generatePersonTaskQueryAccordingToTask(int taskID) //for listing assignees in meeting mode
    {
        string query = "SELECT Name, Surname FROM PERSON WHERE PersonID in (SELECT PersonID FROM PERSONTASK WHERE TaskID=" + "'" + taskID + "'" + ");";

        return query;
    }

    public string generatePersonTaskQueryAccordingToTaskSharing(int personID1, int personID2) //for listing shared tasks in operation mode
    {
        string query = "SELECT TaskName FROM TASK WHERE TaskID in (SELECT TaskID FROM PERSONTASK WHERE PersonID = '" + personID1 + "'" + " INTERSECT SELECT TaskID FROM PERSONTASK WHERE PersonID = '" + personID2 + "')";

        return query;
    }

    public string generatePersonTaskQueryAccordingToPerson(int personID) //for listing current tasks in operation mode 
    {
        string query = "SELECT TaskType, TaskName FROM TASK WHERE TaskID in (SELECT TaskID FROM PERSONTASK WHERE PersonID=" + "'" + personID + "')";

        return query;
    }

    public string generatePersonTaskQueryAccordingToPersonForWorkload(int personID) //for listing current tasks in operation mode 
    {
        string query = "SELECT Workload FROM PERSONTASK WHERE PersonID=" + "'" + personID + "'";

        return query;
    }
}
