using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack2 : MonoBehaviour, IBossAttack
{
    //vars
    public GameObject[] Lasers;
    
    public GameObject[] Warnings;

    private int currentLaser;

    [SerializeField] private float HangTime = 1.5f;

    IEnumerator ShootLasers() //SHOOT THE LASERS
    {
        Lasers[currentLaser].SetActive(true);
        yield return new WaitForSeconds(HangTime);
        Lasers[currentLaser].SetActive(false);
    }

    void TakeDamage()
    {

    }

    IEnumerator DisplayWarnings() //DISPLAY THE WARNINGS
    {
        Warnings[currentLaser].SetActive(true);
        yield return new WaitForSeconds(HangTime);
        Warnings[currentLaser].SetActive(false);

    }

    IEnumerator AttackInterval()
    {
        StartCoroutine (DisplayWarnings());

        yield return new WaitForSeconds(HangTime + 0.2f);

        StartCoroutine(ShootLasers());
    }

    public void PerformAttack()
    {
        currentLaser = Random.Range(0, Lasers.Length);
        StartCoroutine(AttackInterval());
    }
}
