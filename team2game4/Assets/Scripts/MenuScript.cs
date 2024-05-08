using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    public void MouseLeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            src.PlayOneShot(btnClickSfx,1f);
        }
    }

    public static string prevScene;
    public static void changeScene(string name)
    {
        prevScene = SceneManager.GetActiveScene().name;
        GameManager.gm.stomachMeter = 50;
        SceneManager.LoadScene(name);
    }
    public void ChangeScene(string name)
    {
        //src.PlayOneShot(btnClickSfx);
        prevScene = SceneManager.GetActiveScene().name;
        gm.stomachMeter = 50;
        SceneManager.LoadScene(name); 
    }
    public void ChangeScene(int index)
    {
        //src.PlayOneShot(btnClickSfx);
        prevScene = SceneManager.GetActiveScene().name;
        gm.stomachMeter = 50;
        SceneManager.LoadScene(index); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonHover()
    {
        //play hover sfx
        src.PlayOneShot(btnHoverSfx,0.7f);

        //Grow btn
    }
    public void ButtonNotHover()
    {
        //Shrink button to normal size
    }

    public void PlayWithPreset(int preset) //0 standard, 1 evil, 2 ez, 3 custom
    {
        gm.StartWithPreset(preset);
        ChangeScene("SampleScene");
    }

    public void OpenPopup(GameObject _group)
    {
        _group.SetActive(true);
    }
    public void ClosePopup(GameObject _group)
    {
        _group.SetActive(false);
    }
}
