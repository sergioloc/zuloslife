using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagatioController : MonoBehaviour
{
    public Animator animCagatio;
    private int phase;
    public GameObject buttonAttack;

    void OnEnable(){
        phase = LevelTwoValues.phase;
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
    }

    void OnDisable(){
        animCagatio.SetBool("Fire", false); 
    }

    void Update()
    {
        phase = LevelTwoValues.phase;
        if (phase == 10){
            Debug.Log("eee");
            animCagatio.SetTrigger("Ulti");
        }
    }

    IEnumerator EnableAttackButton(float sec)
    {
        yield return new WaitForSeconds(sec);
        buttonAttack.SetActive(true);
        StartCoroutine(DisableAttackButton());
    }

    IEnumerator DisableAttackButton()
    {
        yield return new WaitForSeconds(3f);
        buttonAttack.SetActive(false);
    }
}
