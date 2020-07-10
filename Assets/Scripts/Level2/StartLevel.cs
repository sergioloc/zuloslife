using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public GameObject spawner;
    public Animator animKekeo;
    public AudioSource music;


    void Start()
    {
        animKekeo.SetTrigger("Intro");  
        StartCoroutine(ActiveSpawner());
        StartCoroutine(StartMusic());
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(4.5f);
        music.Play();
    }

    IEnumerator ActiveSpawner()
    {
        yield return new WaitForSeconds(3f);
        spawner.SetActive(true);
    }
}
