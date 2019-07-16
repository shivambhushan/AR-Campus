/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    private DatabaseProcessor database = new DatabaseProcessor();
    //orientationMode
    public RawImage image;
    public VideoClip videoToPlay;
    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
    private AudioSource audioSource;
    //orientationMode
    public Text txtName;
    public Text txtTitle;
    public Text txtDepartment;
    public Text txtPersonalInfo;
    public Text txtContact;
    public Text txtSpeciality;
    public Text txtDevTeam;
    public Text txtEvents;
    public Toggle button;
    public int counter = 0;

    //systemDashboardTask
    public Text txtTaskCode;
    public Text txtStatus;
    public Text txtTaskName;
    public Text txtTaskDefinition;
    public Text txtPlannedStartDate;
    public Text txtStartDate;
    public Text txtEndDate;
    public Text txtWarning;
    public Text txtAssignees;
    private string TFSLink = "";
    public Toggle TFSButton;

    private int personIDForOrientationMode = -1;
    private int personIDForOperationMode = -1;
    private int taskIDForMeetingMode = -1;
    private int loginId;

    //operation
    public Text txtCurrentTasks;
    public Text txtNumOfFeats;
    public Text txtNumOfBugs;
    public Text txtNumOfImps;
    public Text txtTotalWorkload;
    public Text txtSharedTasks;

    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            database.Connect();
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "OrientationMode")
        {
            this.txtEvents.text = "";

            DateTime date = DateTime.Today;
            string sentence = date.ToShortDateString();
            String[] breakApart = sentence.Split('/');
           
            DataTable personDT;
            DataTable eventDT;

            string generatedQueryWithEvent;
            string generatedQueryWithPerson;

            PERSON foundedPerson = new PERSON();
            EVENT foundedEvent = new EVENT();

            generatedQueryWithPerson = foundedPerson.generatePersonQuery(mTrackableBehaviour.TrackableName);       
            personDT = database.GetData(generatedQueryWithPerson);
            displayOrientationData(personDT);

            foreach (DataRow row in personDT.Rows)
            {
             
                this.personIDForOrientationMode = int.Parse(row["PersonID"].ToString());
                break;
            }
            if (this.personIDForOrientationMode != -1)
            {
                generatedQueryWithEvent = foundedEvent.generateEventQuery((this.personIDForOrientationMode));
                eventDT = database.GetData(generatedQueryWithEvent);
                displayEvents(eventDT, breakApart);
            }          
        }

        else if (scene.name == "SystemDashboard")
        {
            DataTable mySysDashDT;
            string generatedQueryWithPerson;
            string generatedQueryWithTask;

            TASK foundedTask = new TASK();
            PERSON foundedPerson = new PERSON();

            generatedQueryWithPerson = foundedPerson.generatePersonQuery(mTrackableBehaviour.TrackableName);
            generatedQueryWithTask = foundedTask.generateTaskQuery(mTrackableBehaviour.TrackableName);

            mySysDashDT = database.GetData(generatedQueryWithPerson);
            if (mySysDashDT.Rows.Count>0)
            {
                foreach(DataRow row in mySysDashDT.Rows)
                {
                    this.personIDForOperationMode = int.Parse(row["PersonID"].ToString());
                    break;
                }
                if(this.personIDForOperationMode!=-1)
                {
                    displayOperationData(this.personIDForOperationMode);
                }
                
            }
            else
            {
                mySysDashDT = database.GetData(generatedQueryWithTask);
                foreach (DataRow row in mySysDashDT.Rows)
                {
                    this.taskIDForMeetingMode = int.Parse(row["TaskID"].ToString());
                    break;
                }
                if (this.taskIDForMeetingMode != -1)
                {
                    TFSButton.isOn = true;
                    displayTaskData(mTrackableBehaviour.TrackableName);
                }
            }           
        }
    }

    public void displayOrientationData(DataTable person)
    {
     
        foreach (DataRow row in person.Rows)
        {
            this.txtName.text = row["Name"].ToString() + " " + row["Surname"].ToString();
            this.txtTitle.text = row["Title"].ToString();
            this.txtDepartment.text = row["Department"].ToString();
            this.txtPersonalInfo.text = row["PersonalInfo"].ToString();
            this.txtContact.text = row["OutlookMail"].ToString();
            this.txtSpeciality.text = row["Speciality"].ToString();
            this.txtDevTeam.text = row["Team"].ToString();
        }
    }

    public void displayEvents(DataTable eventDT, String[] todaysDate)
    {
        string day = todaysDate[0];
        string month = todaysDate[1];
        string year = todaysDate[2];

        string sDay, sMonth, sYear;

        foreach (DataRow row in eventDT.Rows)
        {          
            string dbDate = row["EventDate"].ToString();
            string[] dbDateTime = dbDate.Split(' ');

            string dayMonthYear = dbDateTime[0];
            string time = dbDateTime[1];
            time = time.Remove(time.Length - 3);
            string[] date = dayMonthYear.Split('/');

            sDay = date[0];
            sMonth = date[1];
            sYear = date[2];

            if (sYear == year && sMonth == month && sDay == day)
            {
                this.txtEvents.text += time + " " + row["Header"].ToString() + " \n";
            }
        }
    }

    public void displayOperationData(int personID)
    {
        string loginEmail = Login.getMail();
        string loginPassword= Login.getPassword();
        PERSON myPerson = new PERSON();
        DataTable personInfo;
        personInfo = database.GetData(myPerson.generatePersonQuery(loginEmail, loginPassword));
        foreach (DataRow row in personInfo.Rows)
        {
            this.loginId = Int32.Parse(row["PersonID"].ToString());          
        }

        PERSONTASK myPersonTask = new PERSONTASK();
        DataTable currentTasksDT;
        DataTable currentTasksDT2Workload;
        DataTable sharedTasksDT;
        int numOfFeats;
        int numOfBugs;
        int numOfImp;
        int totalWorkload;

        currentTasksDT = database.GetData(myPersonTask.generatePersonTaskQueryAccordingToPerson(personID));
        this.txtCurrentTasks.text = "";
        numOfFeats = 0;
        numOfBugs = 0;
        numOfImp = 0;
        foreach (DataRow row in currentTasksDT.Rows)
        {
            if (row["TaskType"].ToString().Equals("1"))
            {
                numOfFeats++;
            }
            else if(row["TaskType"].ToString().Equals("2"))
            {
                numOfBugs++;
            }
            else if (row["TaskType"].ToString().Equals("3"))
            {
                numOfImp++;
            }
            this.txtCurrentTasks.text += row["TaskName"].ToString();
            this.txtCurrentTasks.text += "\n";
        }

        currentTasksDT2Workload = database.GetData(myPersonTask.generatePersonTaskQueryAccordingToPersonForWorkload(personID));
        totalWorkload = 0;
        foreach (DataRow row in currentTasksDT2Workload.Rows)
        {
            totalWorkload += int.Parse(row["Workload"].ToString());
        }

        this.txtNumOfFeats.text = numOfFeats.ToString();
        this.txtNumOfBugs.text = numOfBugs.ToString();
        this.txtNumOfImps.text = numOfImp.ToString();
        this.txtTotalWorkload.text = totalWorkload.ToString() + " hours";

        sharedTasksDT = database.GetData(myPersonTask.generatePersonTaskQueryAccordingToTaskSharing(loginId, personID));
        this.txtSharedTasks.text = "";
        foreach (DataRow row in sharedTasksDT.Rows)
        {
            this.txtSharedTasks.text += row["TaskName"].ToString();
            this.txtSharedTasks.text += "\n";
        }
    }

    public void displayTaskData(string uniquePhotoName)
    {
        DateTime today = DateTime.Today;
        string sentence = today.ToShortDateString();
        string sentence2;
        String[] breakApart = sentence.Split('/');
        String[] dateFromDB;
        int dbDay, dbMonth, dbYear;
        int tDay, tMonth, tYear;
        int dayCalcDB = 0, dayCalcToday = 0;

        TASK foundTask = new TASK();
        PERSONTASK personTaskForAssignees = new PERSONTASK();
        DataTable taskInfoDT;
        DataTable taskAssigneesDT;

        taskInfoDT = database.GetData(foundTask.generateTaskQuery(uniquePhotoName));
        this.txtWarning.text = "";
        foreach (DataRow row in taskInfoDT.Rows)
        {
            this.txtTaskCode.text = row["TaskCode"].ToString();
            this.txtTaskName.text = row["TaskName"].ToString();
            this.txtStatus.text = row["TaskProgress"].ToString();
            this.txtTaskDefinition.text = row["TaskDetail"].ToString();
            this.txtPlannedStartDate.text = row["PlannedStartDate"].ToString();
            this.txtStartDate.text = row["StartDate"].ToString();
            this.txtEndDate.text = row["EndDate"].ToString();
            this.TFSLink = row["TFSLink"].ToString();

            sentence2 = row["PlannedStartDate"].ToString();
            dateFromDB = sentence2.Split('-');
            dbDay = int.Parse(dateFromDB[2]); dbMonth = int.Parse(dateFromDB[1]); dbYear = int.Parse(dateFromDB[0]);
            dayCalcDB = dbDay + dbMonth * 30;
            tDay = int.Parse(breakApart[2]); tMonth = int.Parse(breakApart[1]); tYear = int.Parse(breakApart[0]);
            dayCalcToday = tDay + tMonth * 30;
            if (this.txtStatus.text.Equals("To Do") && dayCalcToday>dayCalcDB)
            {
                this.txtWarning.text = "Planned start date is passed!";
            }
            else
            {
                this.txtWarning.text = "";
            }
        }

        taskAssigneesDT = database.GetData(personTaskForAssignees.generatePersonTaskQueryAccordingToTask(this.taskIDForMeetingMode));
        this.txtAssignees.text = "";
        foreach (DataRow row in taskAssigneesDT.Rows)
        {
            this.txtAssignees.text += row["Name"].ToString();
            this.txtAssignees.text += " ";
            this.txtAssignees.text += row["Surname"].ToString();
            this.txtAssignees.text += "\n";
        }
    }

    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
        if (videoPlayer != null && videoPlayer.isPlaying)
            stopVideo();
    }

    public void openTFS()
    {
        Application.OpenURL(this.TFSLink);
        TFSButton.isOn = true;
    }

    IEnumerator playVideo()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();
        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        Debug.Log("Playing Video");
        //while (videoPlayer.isPlaying)
        //{
        //    Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
        //    yield return null;
        //}

        Debug.Log("Done Playing Video");
    }

    public void stopVideo()
    {
        //videoPlayer = gameObject.AddComponent<VideoPlayer>();
        //audioSource = gameObject.AddComponent<AudioSource>();
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            audioSource.Stop();
        }
        counter = 0;
    }

    public void playPauseVideo()
    {
        counter++;

        if(counter==1)
        {
            StartCoroutine(playVideo());
        }

        else if (counter % 2 == 0)
        {
            videoPlayer.Pause();
            audioSource.Pause();
            button.isOn = true;
        }

        else
        {
            videoPlayer.Play();
            audioSource.Play();
            button.isOn = false;
        }
    }

    #endregion // PRIVATE_METHODS
}
