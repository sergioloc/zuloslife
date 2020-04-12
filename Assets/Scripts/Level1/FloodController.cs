using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FloodController : MonoBehaviour
{
    public GameObject door2, part1;
    public ParticleSystem bubblesParticle;
    private bool activated = false;
    private Animator animator;
    public Animator animAlarm;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


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
            part1.SetActive(true);
            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(3f);
        activated = true;
        bubblesParticle.Play();
        animator.SetTrigger("Start");
        animAlarm.SetTrigger("Start");
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        CameraController.instance.ModifyZoom(5.15f);
        animator.SetTrigger("Stop");
        animAlarm.SetTrigger("Stop");
        activated = false;
        door2.SetActive(false);
        bubblesParticle.Stop();
    }
}
