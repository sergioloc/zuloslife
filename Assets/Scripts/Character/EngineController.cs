using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    public GameObject laser;
    public GameObject eyeParticle1, eyeParticle2;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
                StartLaserAttack();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
                FinishLaserAttack();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
                StartFingerAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
                StartGunAttack();
        }
    }

    private void StartLaserAttack(){
        eyeParticle1.SetActive(true);
        eyeParticle2.SetActive(true);
        StartCoroutine(Wait());
    }

    private void FinishLaserAttack(){
        eyeParticle1.SetActive(false);
        eyeParticle2.SetActive(false);
        laser.SetActive(false);
    }

    private void StartFingerAttack(){
        anim.SetBool("Finger", true);
        StartCoroutine(FinishFingerAttack());
    }

    private void StartGunAttack(){
        anim.SetBool("Gun", true);
        StartCoroutine(FinishGunAttack());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        laser.SetActive(true);
    }

    IEnumerator FinishFingerAttack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Finger", false);
    }

    IEnumerator FinishGunAttack()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Gun", false);
    }

}
