using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagatioController : MonoBehaviour
{
    private Animator animCagatio;
    private int phase;
    public GameObject buttonAttack;

    void Start()
    {
        animCagatio = GetComponent<Animator>();
    }

    void OnEnable(){
        phase = TrycicleLevelValues.phase;
        if (phase == 2){
            StartCoroutine(EnableAttackButton(9f));
        }
        else if (phase == 4){
            StartCoroutine(EnableAttackButton(9f));
        }
        else if (phase == 6){
            StartCoroutine(EnableAttackButton(8f));
        }
        else if (phase == 9){
            animCagatio.SetBool("Claw", true);
        }
        else if (phase == 10){
            animCagatio.SetBool("Claw", false);
        }
    }

    void OnDisable(){
        animCagatio.SetBool("Fire", false); 
    }

    void Update()
    {

    }

    IEnumerator EnableAttackButton(float sec)
    {
        yield return new WaitForSeconds(sec);
        buttonAttack.SetActive(true);
        StartCoroutine(DisableAttackButton());
    }

    public void Attack(){
        animCagatio.SetBool("Fire", true);
        buttonAttack.SetActive(false);
    }

    IEnumerator DisableAttackButton()
    {
        yield return new WaitForSeconds(3f);
        buttonAttack.SetActive(false);
    }
}
