using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorCombat : MonoBehaviour
{
    public Animator animEngine, animCagatio, animFeeder;
    public GameObject laser, eyeParticle1, eyeParticle2;

    public Transform zakoPosition;

    private bool isLaser, isSpikes, isFeeded, feeding, attacking;
    private int lastAttack;
    public float timeBtwAttack = 2f;
    public static bool wait;
    
    void Start()
    {
        isLaser = isSpikes = isFeeded = feeding = wait = attacking = false;
    }

    void Update()
    {
        UpdatePhase();

        if (!feeding && !attacking){
            StartCoroutine(Attack());
        }

        /* Fire defense
        if (zakoPosition.position.y == 0 && isLaser){
            animCagatio.SetBool("Fire", true);
        }
        else {
            animCagatio.SetBool("Fire", false);
        }

        // Shield defense
        if (zakoPosition.position.y == -3 && isSpikes){
            animEngine.SetBool("SpikesFail", true);
            animCagatio.SetBool("Shield", true);
        }
        else {
            animCagatio.SetBool("Shield", false);
        }*/
    }

    // General -----------------------------------

    IEnumerator Attack(){
        yield return new WaitForSeconds(1f);
        attacking = true;
        int rand = Random.Range(1, 3);

        if (rand == 1){
            if (rand != lastAttack){
                EngineStartLaser();
                lastAttack = 1;
            }
            else{
                EngineStartSpikes();
                lastAttack = 2;
            }
        }
        else if (rand == 2){
            if (lastAttack != 2){
                EngineStartSpikes();
                lastAttack = 2;
            }
            else {
                EngineStartGun();
                lastAttack = 3;
            }
        }
        else if (rand == 3){
            if (lastAttack != 3){
                EngineStartGun();
                lastAttack = 3;
            }
            else{
                EngineStartLaser();
                lastAttack = 1;
            }
        }
    }

    IEnumerator Feed()
    {
        yield return new WaitForSeconds(2f);
        animEngine.SetTrigger("Feed");
        animFeeder.SetTrigger("Feed");
    }

    private void UpdatePhase(){
        if (TrycicleLevelValues.phase == 4)
        {
            if (!isFeeded){
                StartCoroutine(Feed());
                isFeeded = true;
            }
            feeding = true;
        }
        else{
            feeding = false;
        }
    }


    // Engine ------------------------------------

    // Laser
    private void EngineStartLaser(){
        isLaser = true;
        animEngine.SetBool("Laser", true);
        StartCoroutine(EngineStopLaser());
    }

    IEnumerator EngineStopLaser()
    {
        yield return new WaitForSeconds(1.1f);
        isLaser = false;
        attacking = false;
        animEngine.SetBool("Laser", false);
        //animCagatio.SetBool("Fire", false);
    }

    // Spikes
    private void EngineStartSpikes(){
        isSpikes = true;
        animEngine.SetBool("Spikes", true);
        StartCoroutine(EngineStopSpikes());
    }

    IEnumerator EngineStopSpikes()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("stop");
        isSpikes = false;
        attacking = false;
        animEngine.SetBool("Spikes", false);
        animEngine.SetBool("SpikesFail", false);
        animCagatio.SetBool("Shield", false);
    }

    // Gun
    private void EngineStartGun(){
        animEngine.SetBool("Gun", true);
        StartCoroutine(EngineStopGun());
    }

    IEnumerator EngineStopGun()
    {
        yield return new WaitForSeconds(1f);
        animEngine.SetBool("Gun", false);
        attacking = false;
    }

}