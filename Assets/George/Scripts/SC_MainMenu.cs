using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject exitScreen;

    // hide controls and exit screens on load
    public void Start()
    {
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(false);
        }
        if (exitScreen != null)
        {
            exitScreen.SetActive(false);
        }
    }

    // load level
    public void OnPlayPressed()
    {
        SceneManager.LoadScene(1);
    }

    // open controls menu
    public void OnControlsPressed()
    {
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(true);
        }
    }

    // open exit menu or exit game - if menu already open
    public void OnExitPressed()
    {
        if (exitScreen != null)
        {
            if (exitScreen.activeSelf)
            {
                Debug.Log("Quiting Game");
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
        if (controlsScreen != null)
        {
            controlsScreen.SetActive(false);
        }

        if (exitScreen != null)
        {
            exitScreen.SetActive(false);
        }
    }
}
