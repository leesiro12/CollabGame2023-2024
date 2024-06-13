using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSplashVideo : MonoBehaviour
{
    public float delay;

    void Start()
    {
        StartCoroutine(WaitForVideoComplete());
    }

    IEnumerator WaitForVideoComplete()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
