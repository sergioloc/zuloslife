
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActionController : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public GameObject buttonAttack;
    private Animator animator;
    private int phase;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable(){
        phase = LevelTwoValues.phase;
        if (phase == 1){
            animator.SetTrigger("Evolution");
            StartCoroutine(EnableAttackButton(7f));
        }
        else if (phase == 2){
            StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 3){
            //StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 4){
            //StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 5){
            //StartCoroutine(EnableAttackButton(8f));
        }
        else if (phase == 6){
            //StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 7){
            //SuitUp();
        }
        else if (phase == 8){
            //StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 9){
            //StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 10){
            //transform.position = new Vector2(1f, 0f);
            //animator.SetTrigger("Ulti");
        }
        else if (phase == 11){
            //
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
        if (phase < 11)
            animator.SetBool("Attack", true);
        else
            animator.SetBool("Ulti", true);
        buttonAttack.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponSoft"))
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
