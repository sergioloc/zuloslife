using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AlarmLights : MonoBehaviour
{
    public GameObject door2, water, bubblesParticle;
    private bool activated = false;
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
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            door2.SetActive(true);
            StartCoroutine(StartLevel());
        }
        else if (collision.gameObject.CompareTag("PlayerDeath") && activated)
        {
            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(3f);
        activated = true;
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
        bubblesParticle.SetActive(false);
    }
}
