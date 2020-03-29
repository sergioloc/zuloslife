using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorCombat : MonoBehaviour
{
    public Animator animEngine, animCagatio, animFeeder;
    public GameObject laser, eyeParticle1, eyeParticle2;

    public Transform zakoPosition;

    private bool isLaser, isFinger;
    private int lastAttack;
    
    void Start()
    {
        isLaser = isFinger = false;
        Attack();
    }

    void Update()
    {
        if (zakoPosition.position.y == 0 && isLaser){
            CagatioStartFire();
        }
        else {
            CagatioStopFire();
        }

        if (zakoPosition.position.y == -3 && isFinger){
            animEngine.SetBool("SpikesFail", true);
            CagatioStartShield();
            StartCoroutine(EngineStopSpikesFail());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animEngine.SetTrigger("Feed");
            animFeeder.SetTrigger("Feed");
        }

    }

    // General -----------------------------------

    private void Attack(){
        int rand = Random.Range(1, 3);

        if (rand == 1){
            if (rand != lastAttack){
                EngineStartLaserAttack();
                lastAttack = 1;
            }
            else{
                EngineStartFingerAttack();
                lastAttack = 2;
            }
        }
        else if (rand == 2){
            if (lastAttack != 2){
                EngineStartFingerAttack();
                lastAttack = 2;
            }
            else {
                EngineStartGunAttack();
                lastAttack = 3;
            }
        }
        else if (rand == 3){
            if (lastAttack != 3){
                EngineStartGunAttack();
                lastAttack = 3;
            }
            else{
                EngineStartLaserAttack();
                lastAttack = 1;
            }
        }
        StartCoroutine(NextAttack());
    }

    IEnumerator NextAttack()
    {
        yield return new WaitForSeconds(4f);
        Attack();
    }


    // Engine ------------------------------------

    private void EngineStartLaserAttack(){
        isLaser = true;
        eyeParticle1.SetActive(true);
        eyeParticle2.SetActive(true);
        StartCoroutine(EngineActiveLaser());
        StartCoroutine(EngineFinishLaserAttack());
    }

    private void EngineStartFingerAttack(){
        isFinger = true;
        animEngine.SetBool("Spikes", true);
        StartCoroutine(EngineFinishFingerAttack());
    }

    private void EngineStartGunAttack(){
        animEngine.SetBool("Gun", true);
        StartCoroutine(EngineFinishGunAttack());
    }

    IEnumerator EngineActiveLaser()
    {
        yield return new WaitForSeconds(0.5f);
        laser.SetActive(true);
    }

    IEnumerator EngineFinishLaserAttack()
    {
        yield return new WaitForSeconds(3f);
        isLaser = false;
        eyeParticle1.SetActive(false);
        eyeParticle2.SetActive(false);
        laser.SetActive(false);
    }

    IEnumerator EngineFinishFingerAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isFinger = false;
        animEngine.SetBool("Spikes", false);
    }

    IEnumerator EngineFinishGunAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animEngine.SetBool("Gun", false);
    }

    IEnumerator EngineStopSpikesFail()
    {
        yield return new WaitForSeconds(2f);
        animEngine.SetBool("SpikesFail", false);
    }

    // Cagatio ----------------------------------

    private void CagatioStartShield(){
        animCagatio.SetBool("Shield", true);
        StartCoroutine(CagatioStopShield());
    }

    private void CagatioStartFire(){
        animCagatio.SetBool("Fire", true);
    }

    private void CagatioStopFire(){
        animCagatio.SetBool("Fire", false);
    }

    IEnumerator CagatioStopShield()
    {
        yield return new WaitForSeconds(0.5f);
        animCagatio.SetBool("Shield", false);
    }
}
