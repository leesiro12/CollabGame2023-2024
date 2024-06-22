using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossAreaControl : MonoBehaviour
{
    [SerializeField] private int phase = 1;

    [SerializeField] private Camera cam;
    private CameraControler camScript;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawn;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private GameObject[] walls;

    [SerializeField] private GameObject boss;
    [SerializeField] private BossHealth bossHealthScript;
    [SerializeField] private BossMover bossMoverScript;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        if (cam != null )
        {
            camScript = cam.GetComponent<CameraControler>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && boss == null)
        {
            boss = Instantiate(bossPrefab, bossSpawn.position, Quaternion.identity, null);
            boss.SetActive(true);

            bossHealthScript = boss.GetComponentInChildren<BossHealth>();
            bossHealthScript.onBossDeath += EndPhase;

            bossMoverScript = boss.GetComponentInChildren<BossMover>();
            if (bossMoverScript != null)
            {
                bossMoverScript.SetPhase(phase);
            }

            if (camScript != null)
            {
                camScript.SetFixedPosition(cameraPos);
            }

            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
            }
        }
    }

    private void EndPhase()
    {
        bossHealthScript.onBossDeath -= EndPhase;

        Destroy(boss);
        
        foreach (GameObject wall in walls)
        {
            wall.SetActive(false);
        }

        StartCoroutine(WaitAndRestoreCam());
    }

    IEnumerator WaitAndRestoreCam()
    {
        yield return new WaitForSeconds(1);
        MAudioManager.instance.PlayMusic("BGMLava");
        if (camScript != null)
        {
            camScript.ReleaseFixedPosition();
        }

        Destroy(this);
    }
}
