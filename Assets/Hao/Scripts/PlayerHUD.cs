using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public int health;
    public int numOfHearts;
    //public Animator anim;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeSpeed;

    //public Animator anim;
    private void Awake()
    {
        hearts = GetComponentsInChildren<Image>();
    }

    private void OnEnable()
    {
        HealthScript.onHealthChange += HealthChange;
        HealthScript.onPlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        HealthScript.onHealthChange -= HealthChange;
        HealthScript.onPlayerDeath -= PlayerDeath;
    }

    private void HealthChange(int newHealth)
    {
        health = newHealth;

        // cap health value to num of hearts
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            // show full/empty heart according to health
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // only show heart if within number of hearts limit
            hearts[i].enabled = (i < numOfHearts);
        }
    }

    private void PlayerDeath()
    {
        HealthChange(0);
        
        StartCoroutine(FadeToBlack());
        // any extra UI to display
    }

    // screen fade and slow time, then reload checkpoint
    IEnumerator FadeToBlack()
    {
        yield return new WaitForSeconds(1f);
        float startTime = Time.realtimeSinceStartup;
        if (blackScreen != null)
        {
            while (blackScreen.color.a < 1)
            {
                float newAlpha = fadeSpeed * (Time.realtimeSinceStartup - startTime);
                if (newAlpha > 1)
                {
                    newAlpha = 1;
                }
                blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, newAlpha);
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.5f);

        //restart from checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
