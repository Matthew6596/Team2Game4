using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static string prevScene;
    public static void ChangeScene(string name)
    { 
        prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name); 
    }
    public static void ChangeScene(int index)
    {
        prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(index); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
