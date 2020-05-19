using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private Animator animEngine;
    public Animator animFeeder;
    public GameObject implosion, bullet, spikes;
    public Transform bulletPoint, spikesPoint;

    private bool isFeeded, feeding;
    private int lastAttack;
    public float timeBtwAttack = 2f;
    public static bool wait;
    
    void Start()
    {
        animEngine = GetComponent<Animator>();
        feeding = wait = false;
    }

    void OnEnable(){
        StartCoroutine(Attack());
    }

    void OnDisable(){
        StopCoroutine(Attack());
    }

    IEnumerator Attack(){
        while (true) {
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

    public void ShootBullet(){
        Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
    }

    public void ShootSpikes(){
        Instantiate(spikes, spikesPoint.position, spikesPoint.rotation);
    }

    IEnumerator Feed()
    {
        yield return new WaitForSeconds(2f);
        animEngine.SetTrigger("Feed");
        animFeeder.SetTrigger("Feed");
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(9.5f);
        gameObject.SetActive(false);
    }

    public void UpdateEngineState(){
        if (LevelTwoValues.phase == 4) {
            StartCoroutine(Feed());
        }
        else if (LevelTwoValues.phase == 10) {
            implosion.SetActive(true);
            StartCoroutine(Die());
        }
    }
}