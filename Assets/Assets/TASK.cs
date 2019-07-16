using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TASK : MonoBehaviour
{

    private int TaskID;
    private string TaskCode;
    private int TaskType;
    private string TaskName;
    private string TaskProgress;
    private string TaskDetail;
    private int PorjectID;
    private string ARFotoName;

    public TASK()
    {

    }

    public TASK(int taskID, string taskCode, int taskType, string taskName, string taskProgress, string taskDetail, int porjectID, string aRFotoName)
    {
        TaskID = taskID;
        TaskCode = taskCode;
        TaskType = taskType;
        TaskName = taskName;
        TaskProgress = taskProgress;
        TaskDetail = taskDetail;
        PorjectID = porjectID;
        ARFotoName = aRFotoName;
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

    public string TaskCode1
    {
        get
        {
            return TaskCode;
        }

        set
        {
            TaskCode = value;
        }
    }

    public int TaskType1
    {
        get
        {
            return TaskType;
        }

        set
        {
            TaskType = value;
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

    public string TaskProgress1
    {
        get
        {
            return TaskProgress;
        }

        set
        {
            TaskProgress = value;
        }
    }

    public string TaskDetail1
    {
        get
        {
            return TaskDetail;
        }

        set
        {
            TaskDetail = value;
        }
    }

    public int PorjectID1
    {
        get
        {
            return PorjectID;
        }

        set
        {
            PorjectID = value;
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

    public string generateTaskQuery(string uniquePhotoName)
    {
        string sqlQuery;
        sqlQuery = "SELECT *FROM TASK WHERE ARFotoName=" + "'" + uniquePhotoName + "'";
        return sqlQuery;
    }
}
