using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject exitScreen;
    // hide controls and exit screens on load
    public void Start()
    {
        StartCoroutine(DelayStart());
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(false);
        }
        if (exitScreen != null)
        {
            exitScreen.SetActive(false);
        }
    }
    IEnumerator DelayStart()
    {
        yield return new WaitForEndOfFrame();
        MAudioManager.instance.PlayMusic("MenuBGM");
    }

    // load level
    public void OnPlayPressed()
    {
        MAudioManager.instance.PlaySFX("Click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // open controls menu
    public void OnControlsPressed()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(true);
        }
    }

    public void OnSettingsPressed()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (settingsScreen != null)
        {
            settingsScreen.SetActive(true);
        }
    }

    // open exit menu or exit game - if menu already open
    public void OnExitPressed()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (exitScreen != null)
        {
            if (exitScreen.activeSelf)
            {
                //Debug.Log("Quiting Game");
                Application.Quit();
            }
            else
            {
                exitScreen.SetActive(true);
            }
        }
    }

    // turn off control screen/exit screen to return to main menu
    public void OnReturnPressed()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(false);
        }

        if (settingsScreen != null)
        {
            settingsScreen.SetActive(false);
        }

        if (exitScreen != null)
        {
            exitScreen.SetActive(false);
        }
    }
    public void OnMouseEnter()
    {
        MAudioManager.instance.PlaySFX("ButtonHover");
    }
}
