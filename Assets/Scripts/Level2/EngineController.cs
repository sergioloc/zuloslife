using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private Animator animEngine;
    public Animator animFeeder;

    private bool isFeeded, feeding, attacking;
    private int lastAttack;
    public float timeBtwAttack = 2f;
    public static bool wait;
    
    void Start()
    {
        animEngine = GetComponent<Animator>();
        feeding = wait = attacking = false;
    }

    void Update()
    {
        UpdatePhase();

        if (!feeding && !attacking){
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack(){
        yield return new WaitForSeconds(1f);
        attacking = true;
        int rand = Random.Range(1, 3);

        if (rand == 1){
            if (lastAttack != 1){
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
            if (!feeding){
                StartCoroutine(Feed());
                feeding = true;
            }
        }
        else{
            feeding = false;
        }
    }

    // Laser
    private void EngineStartLaser(){
        animEngine.SetBool("Laser", true);
        StartCoroutine(EngineStopLaser());
    }

    IEnumerator EngineStopLaser()
    {
        yield return new WaitForSeconds(1.1f);
        attacking = false;
        animEngine.SetBool("Laser", false);
    }

    // Spikes
    private void EngineStartSpikes(){
        animEngine.SetBool("Spikes", true);
        StartCoroutine(EngineStopSpikes());
    }

    IEnumerator EngineStopSpikes()
    {
        yield return new WaitForSeconds(1.5f);
        attacking = false;
        animEngine.SetBool("Spikes", false);
        animEngine.SetBool("SpikesFail", false);
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