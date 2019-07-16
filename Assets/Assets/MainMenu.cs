using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void OrientationMode()
    {
        SceneManager.LoadScene("OrientationMode");
    }

    public void SystemDashboard()
    {
        SceneManager.LoadScene("SystemDashboard");
    }

    public void Logout()
    {
        Login.setMail(null);
        Login.setPassword(null);
        SceneManager.LoadScene("Login");
    }

    public void Quit()
    {
        Debug.Log("QUIT !");
        Application.Quit();
    }
}
