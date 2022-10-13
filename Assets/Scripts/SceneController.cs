using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OpenGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenDashboard()
    {
        //SceneManager.LoadScene(0);
    }
}
