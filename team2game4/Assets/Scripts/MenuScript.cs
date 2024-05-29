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

    public static bool muteClick = false;

    Image pauseBtnImg;
    public Sprite pauseSprite,resumeSprite;

    GameManager gm;
    private void Start()
    {
        if (creditsPanel != null)
        {
            creditsPanel.gameObject.SetActive(true);
            creditsEndLocation = creditsPanel.localPosition;
            creditsPanel.localPosition = creditsStartLocation.localPosition;
        }
        pauseBtnImg = GetComponent<Image>();
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
        if (ctx.performed && !muteClick)
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

    public void PauseToggle(GameObject _pauseScrn)
    {
        bool pausingGame = Time.timeScale == 1;
        if (pausingGame) //must pause game
        {
            Time.timeScale = 0;
            _pauseScrn.SetActive(true);
            //Change pause icon to resume icon
            pauseBtnImg.sprite = resumeSprite;
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = resumeSprite;
            //Move btn to center of screen
            transform.localPosition = Vector3.right*(((RectTransform)transform).rect.width/2);
            //Set text
            transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "Resume";
        }
        else //unpause game
        {
            Time.timeScale = 1;
            _pauseScrn.SetActive(false);
            //Change resume icon to pause icon
            pauseBtnImg.sprite = pauseSprite;
            transform.GetChild(0).gameObject.GetComponent<Image>().sprite = pauseSprite;
            //Move btn to bottom right position
            transform.localPosition = new Vector3(390, -215, 0);
            //Set text
            transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "";
        }
    }
}
