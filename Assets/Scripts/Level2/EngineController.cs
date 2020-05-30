using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private Animator animEngine;
    public GameObject explosion, bullet, spikes, cagatio, cannons, feeder, wind, kekeo;
    public Transform bulletPoint, spikesPoint;

    private int lastAttack, phase;
    private bool wait;
    
    void Start()
    {
        animEngine = GetComponent<Animator>();
    }

    public void UpdateEngineState(){
        wait = false;
        phase = LevelTwoValues.phase;
        if (phase == 2){
            StartCoroutine(Attack());
        }
        else if (phase == 4){
            animEngine.SetTrigger("Feed");
            feeder.SetActive(true);
            feeder.GetComponent<Animator>().SetTrigger("Feed");
        }
        else if (phase == 6){
            Destroy(feeder);
            StartCoroutine(Attack());
        }
        else if (phase == 7){
            wait = true;
        }
        else if (phase == 9){
            animEngine.SetTrigger("Weak");
        }
        else if (phase == 11){
            StartCoroutine(Die());
        } 
    }

    void OnDisable(){
        animEngine.SetTrigger("Idle");
        StopCoroutine(Attack());
    }

    IEnumerator Attack(){
        while (!wait) {
            yield return new WaitForSeconds(LevelTwoValues.timeBtwSpawn);
            ChooseAttack(Random.Range(1, 3));
        }
    }

    private void ChooseAttack(int i){
        if (i == 1){
            if (lastAttack != 1){
                animEngine.SetTrigger("Laser");
                lastAttack = 1;
            }
            else{
                animEngine.SetBool("Spikes", true);
                lastAttack = 2;
            }
        }
        else if (i == 2){
            if (lastAttack != 2){
                animEngine.SetBool("Spikes", true);
                lastAttack = 2;
            }
            else {
                animEngine.SetBool("Gun", true);
                lastAttack = 3;
            }
        }
        else if (i == 3){
            if (lastAttack != 3){
                animEngine.SetBool("Gun", true);
                lastAttack = 3;
            }
            else{
                animEngine.SetTrigger("Laser");
                lastAttack = 1;
            }
        }
    }

    private IEnumerator Die(){
        Instantiate(explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);
        kekeo.SetActive(false);
        cagatio.SetActive(false);
        cannons.SetActive(false);
        wind.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShootBullet(){
        Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
    }

    public void ShootSpikes(){
        Instantiate(spikes, spikesPoint.position, spikesPoint.rotation);
    }
}