using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROJECT : MonoBehaviour
{

    private int ProjectID;
    private string ProjectName;

    public PROJECT()
    {
    }

    public PROJECT(int projectID1, string projectName1)
    {
        ProjectID1 = projectID1;
        ProjectName1 = projectName1;
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

    public string ProjectName1
    {
        get
        {
            return ProjectName;
        }

        set
        {
            ProjectName = value;
        }
    }


}
