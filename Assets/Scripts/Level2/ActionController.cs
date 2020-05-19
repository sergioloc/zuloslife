﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ActionController : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public GameObject buttonAttack;
    public GameObject chargeParticle, powerParticle;
    public bool isLeft = false;
    private Animator animator;
    private int phase = 0;
    public UnityEvent OnShake, OnShakeLoop, OnShakeLoopStop;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && buttonAttack.activeSelf)
        {
            Attack();
        }
    }

    public void UpdateActionState(){
        phase = LevelTwoValues.phase;
        if (phase == 1){
            StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 2){
            StartCoroutine(EnableAttackButton(8f));
        }
        else if (phase == 3){
            StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 4){
            StartCoroutine(EnableAttackButton(8f));
        }
        else if (phase == 5){
            StartCoroutine(EnableAttackButton(7f));
        }
        else if (phase == 6){
            if (!isLeft) transform.position = new Vector2(transform.position.x, 0);
            animator.SetTrigger("Evolution");
            StartCoroutine(EnableAttackButton(6f));
        }
        else if (phase == 7){
            if (isLeft) transform.position = new Vector2(transform.position.x, 0);
            animator.SetTrigger("Evolution");
        }
        else if (phase == 8){
            StartCoroutine(EnableAttackButton(1f));
        }
        else if (phase == 9){
            animator.SetBool("isEvolved", true);
        }
        else if (phase == 10){
            transform.position = new Vector2(transform.position.x, 0);
            animator.SetTrigger("Ulti");
            if (isLeft) Instantiate(chargeParticle, new Vector2(5f, 0f), Quaternion.identity);
            else Instantiate(chargeParticle, new Vector2(24f, 2f), Quaternion.identity);
        }
        else if (phase == 11){
            //end
        }
    }

    IEnumerator EnableAttackButton(float sec)
    {
        yield return new WaitForSeconds(sec);
        buttonAttack.SetActive(true);
        yield return new WaitForSeconds(3f);
        buttonAttack.SetActive(false);
    }

    public void Attack(){
        animator.SetBool("Attack", true);
        buttonAttack.SetActive(false);
    }

    public void ShowPowerParticle(){
        if (isLeft) Instantiate(powerParticle, new Vector2(-5f, 0f), Quaternion.identity);
        else Instantiate(powerParticle, new Vector2(14f, 2f), Quaternion.identity);
    }

    public void ShakeScreen(){
        OnShake.Invoke();
    }

    public void StartShakeScreen(){
        OnShakeLoop.Invoke();
    }

    public void StopShakeScreen(){
        OnShakeLoopStop.Invoke();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponSoft") || (collision.gameObject.CompareTag("Laser")))
        {
            //LevelTwoValues.health--;
            healthSlider.value = LevelTwoValues.health;
            if (LevelTwoValues.health <= 0){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                LevelTwoValues.health = 3;
            }
        }
    }
}
