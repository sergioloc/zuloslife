using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public GameObject spawner, buttons;
    public Animator animKekeo;
    public AudioSource music;


    void Start()
    {
        if (LevelTwoValues.isFinalPhase)
            animKekeo.SetTrigger("Evolution");
        else
            animKekeo.SetTrigger("Intro");
        StartCoroutine(ActiveButtons());
        StartCoroutine(StartMusic());
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(5f);
        music.Play();
        spawner.SetActive(true);
    }

    IEnumerator ActiveButtons()
    {
        yield return new WaitForSeconds(3f);
        buttons.SetActive(true);
    }
}
