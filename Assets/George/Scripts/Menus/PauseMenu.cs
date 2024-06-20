using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject restartScreen;
    [SerializeField] private GameObject restartCheckpointScreen;

    // deactivate menus at start
    void Start()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }

        if (exitScreen != null)
        {
            exitScreen.SetActive(false);
        }

        if (restartScreen != null)
        {
            restartScreen.SetActive(false);
        }
        
        if (restartCheckpointScreen != null)
        {
            restartCheckpointScreen.SetActive(false);
        }
    }

    // toggle between pause menu open/closed
    public void OnPause()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (pauseScreen != null)
        {
            if (!pauseScreen.activeSelf)
            {
                Time.timeScale = 0.0f;
                pauseScreen.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                pauseScreen.SetActive(false);

                // also close exit/restart screens if open
                if (exitScreen != null && exitScreen.activeSelf)
                {
                    exitScreen.SetActive(false);
                }
                if (restartScreen != null && restartScreen.activeSelf)
                {
                    restartScreen.SetActive(false);
                }
                if (restartCheckpointScreen != null && restartCheckpointScreen.activeSelf)
                {
                    restartCheckpointScreen.SetActive(false);
                }
            }
        }
    }

    // open exit menu/exit scene
    public void OnExit()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (exitScreen != null)
        {
            // if not open, open exit menu
            if (!exitScreen.activeSelf)
            {
                // also close exit screen is open
                if (pauseScreen != null && pauseScreen.activeSelf)
                {
                    pauseScreen.SetActive(false);
                    exitScreen.SetActive(true);
                }
            }
            // if already open, exit to menu
            else
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(2);
            }
        }
    }

    // close exit menu
    public void OnReturn()
    {
        MAudioManager.instance.PlaySFX("Click");
        // if exit screen open, close
        if (exitScreen != null && exitScreen.activeSelf)
        {
            exitScreen.SetActive(false);
        }
        // if restart screen open, close
        else if (restartScreen != null && restartScreen.activeSelf)
        {
            restartScreen.SetActive(false);
        }
        // if restart from checkpoint screen open, close
        else if (restartCheckpointScreen != null && restartCheckpointScreen.activeSelf)
        {
            restartCheckpointScreen.SetActive(false);
        }

        // reopen pause menu
        if (pauseScreen != null)
        {
            if (!pauseScreen.activeSelf)
            {
                pauseScreen.SetActive(true);
            }
        }
    }

    public void OnRestart()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (restartScreen != null)
        {
            // if not open, open restart menu
            if (!restartScreen.activeSelf)
            {
                // also close exit screen is open
                if (pauseScreen != null && pauseScreen.activeSelf)
                {
                    pauseScreen.SetActive(false);
                    restartScreen.SetActive(true);
                }
            }
            // if already open, exit to menu
            else
            {
                Time.timeScale = 1.0f;
                // reset current check point and hasKey status to default values
                CheckpointsManager.lastCheckPointPos = CheckpointsManager.defaultCheckPointPos;
                InventoryCheck.hasKey = false;
                AttackScript.canRangeAttack = false;
                SceneManager.LoadScene(3);
            }
        }
    }

    public void OnRestartCheckpoint()
    {
        MAudioManager.instance.PlaySFX("Click");
        if (restartCheckpointScreen != null)
        {
            // if not open, open exit menu
            if (!restartCheckpointScreen.activeSelf)
            {
                // also close exit screen is open
                if (pauseScreen != null && pauseScreen.activeSelf)
                {
                    pauseScreen.SetActive(false);
                    restartCheckpointScreen.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                //restart from checkpoint
                SceneManager.LoadScene(3);
            }
        }
    }
    public void OnMouseEnter()
    {
        MAudioManager.instance.PlaySFX("ButtonHover");
    }
}
