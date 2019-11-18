using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic2 : MonoBehaviour
{
    public Animator argerichAnim, keroAnim;
    private Animator cameraAnim;
    public ParticleSystem particle;
    public GameObject cagatio, finalEv, cellSparks, cellLeft, wallParticle, door;
    private bool scaped = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraAnim = gameObject.GetComponent<Animator>();
        StartCoroutine(StartCinematic());
    }

    // Update is called once per frame
    void Update()
    {
        if (finalEv.activeSelf && !scaped)
        {
            cameraAnim.SetBool("Shake", false);
            cagatio.SetActive(true);
            finalEv.SetActive(false);
            cellLeft.SetActive(false);
            cellSparks.SetActive(false);
            StartCoroutine(Scape());
        }
    }

    IEnumerator StartCinematic()
    {
        yield return new WaitForSeconds(2f);
        keroAnim.SetBool("Action", true);
        StartCoroutine(StartCellSparks());
        StartCoroutine(StopKero());
    }

    IEnumerator StopKero()
    {
        yield return new WaitForSeconds(1f);
        keroAnim.SetBool("Action", false);
    }

    IEnumerator StartCellSparks()
    {
        yield return new WaitForSeconds(1f);
        cellSparks.SetActive(true);
        argerichAnim.SetBool("Ev", true);
        cameraAnim.SetBool("Shake", true);
        particle.Play();
    }

    IEnumerator Scape()
    {
        yield return new WaitForSeconds(4f);
        cameraAnim.SetBool("Shake", true);
        scaped = true;
        cagatio.GetComponent<Animator>().SetTrigger("Scape");
        wallParticle.SetActive(true);
        door.SetActive(false);
        cameraAnim.SetBool("Shake", false);

    }
}
