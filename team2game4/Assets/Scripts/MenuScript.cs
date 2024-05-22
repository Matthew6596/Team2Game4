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

    public Transform creditsPanel;
    public Transform creditsStartLocation;
    Vector3 creditsEndLocation;
    bool creditsOn = false;

    GameManager gm;
    private void Start()
    {
        if (creditsPanel != null)
        {
            creditsPanel.gameObject.SetActive(true);
            creditsEndLocation = creditsPanel.localPosition;
            creditsPanel.localPosition = creditsStartLocation.localPosition;
        }
        src = GetComponent<AudioSource>();
        gm = GameManager.gm;
        gm.menuScript = this;
    }

    private void Update()
    {
        if(creditsPanel!=null)
            creditsPanel.localPosition = 
            Tween.LazyTween(creditsPanel.localPosition, (creditsOn) ? (creditsEndLocation) : (creditsStartLocation.localPosition), 0.1f);
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
        
        ScreenTransition.instance.ChangeScene(name, 0.1f, .4f);
    }
    public void ChangeScene(string name)
    {
        //src.PlayOneShot(btnClickSfx);
        prevScene = SceneManager.GetActiveScene().name;
        //gm.stomachMeter = 50;
        //SceneManager.LoadScene(name); 
        ScreenTransition.instance.ChangeScene(name, 0.1f, .4f);
    }
    public void ChangeScene(int index)
    {
        //src.PlayOneShot(btnClickSfx);
        prevScene = SceneManager.GetActiveScene().name;
        //gm.stomachMeter = 50;
        //SceneManager.LoadScene(index); 
        ScreenTransition.instance.ChangeScene(SceneManager.GetSceneByBuildIndex(index).name, 0.1f, .4f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonHover()
    {
        //play hover sfx
        src.PlayOneShot(btnHoverSfx,0.7f);
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

    public void ToggleCredits(bool on)
    {
        creditsOn = on;
    }
}
