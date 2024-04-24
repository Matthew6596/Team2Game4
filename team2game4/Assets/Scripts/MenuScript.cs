using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public AudioClip btnHoverSfx,btnClickSfx;
    AudioSource src;

    GameManager gm;
    private void Start()
    {
        src = GetComponent<AudioSource>();
        gm = GameManager.gm;
        gm.menuScript = this;
    }


    public static string prevScene;
    public static void changeScene(string name)
    {
        prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }
    public void ChangeScene(string name)
    {
        //src.PlayOneShot(btnClickSfx);
        prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name); 
    }
    public void ChangeScene(int index)
    {
        //src.PlayOneShot(btnClickSfx);
        prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(index); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonHover()
    {
        //play hover sfx
        src.PlayOneShot(btnHoverSfx);
    }
}
