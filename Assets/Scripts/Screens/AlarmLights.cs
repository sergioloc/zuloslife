using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AlarmLights : MonoBehaviour
{
    public GameObject door2, level1, light1, light2, light3, light4, light5, light6, water, launchers, bubblesParticle;
    private bool check = false, activated = false;
    private Animator waterAnimator;
    public CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        waterAnimator = water.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (check && activated)
        {
            Debug.Log(check);
            check = false;
            StartCoroutine(BlinkLights());
        }
        else if (!activated)
        {
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            light4.SetActive(false);
            light5.SetActive(false);
            light6.SetActive(false);
        }
    }


    IEnumerator BlinkLights()
    {
        yield return new WaitForSeconds(1f);
        if (!light1.activeInHierarchy)
        {
            light1.SetActive(true);
            light2.SetActive(true);
            light3.SetActive(true);
            light4.SetActive(true);
            light5.SetActive(true);
            light6.SetActive(true);
        }
        else
        {
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            light4.SetActive(false);
            light5.SetActive(false);
            light6.SetActive(false);
        }
        check = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            level1.SetActive(false);
            door2.SetActive(true);
            StartCoroutine(StartLevel());
        }
        else if (collision.gameObject.CompareTag("PlayerDeath"))
        {
            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(3f);
        check = true;
        activated = true;
        launchers.SetActive(true);
        bubblesParticle.SetActive(true);
        waterAnimator.SetTrigger("Start");
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        virtualCamera.m_Lens.OrthographicSize = 5.15f;
        waterAnimator.SetTrigger("Stop");
        activated = false;
        door2.SetActive(false);
        launchers.SetActive(false);
        bubblesParticle.SetActive(false);
    }
}
