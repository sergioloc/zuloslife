using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private Animator animEngine;
    public GameObject explosion, bullet, spikes, cagatio, cannons, feeder, wind, kekeo;
    public Transform bulletPoint, spikesPoint;

    private int lastAttack, phase;
    public float timeBtwAttack = 2f;
    private bool wait;
    
    void Start()
    {
        animEngine = GetComponent<Animator>();
    }

    public void UpdateEngineState(){
        wait = false;
        phase = LevelTwoValues.phase;
        if (phase == 4) {
            animEngine.SetTrigger("Feed");
            feeder.GetComponent<Animator>().SetTrigger("Feed");
        }
        else if (phase == 10){
            wait = true;
            Destroy(feeder);
        }
        else if (phase == 11){
            wait = true;
            StartCoroutine(FinalAnimation());
        }
        else {
            StartCoroutine(Attack());
        }
        
    }

    void OnDisable(){
        StopCoroutine(Attack());
    }

    IEnumerator Attack(){
        while (!wait) {
            yield return new WaitForSeconds(timeBtwAttack);
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

    private IEnumerator FinalAnimation(){
        yield return new WaitForSeconds(2f);
        animEngine.SetTrigger("Die");   
    }

    public void ShootBullet(){
        Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
    }

    public void ShootSpikes(){
        Instantiate(spikes, spikesPoint.position, spikesPoint.rotation);
    }

    public void InvokeDieParticles(){
        Instantiate(explosion, transform.position, transform.rotation);
    }

    public void Die(){
        kekeo.SetActive(false);
        cagatio.SetActive(false);
        cannons.SetActive(false);
        wind.SetActive(false);
        gameObject.SetActive(false);
    }
}