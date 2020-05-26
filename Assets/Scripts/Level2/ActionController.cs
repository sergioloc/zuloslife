
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
    public Image colorSlider;
    public GameObject buttonAttack;
    public GameObject chargeParticle, powerParticle;
    public bool isLeft = false; 
    public Color32 healColor;
    private Color32 red;
    private Animator animator;
    private int phase = 0;
    public TimeManager timeManager;
    public UnityEvent OnShake, OnShakeLoop, OnShakeLoopStop;

    void Awake()
    {
        animator = GetComponent<Animator>();
        red = new Color32(207, 28, 28, 255);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && buttonAttack.activeSelf)
        {
            Attack();
        }
    }

    public void UpdateActionState(){
        phase = LevelTwoValues.phase;
        if (gameObject.activeInHierarchy){
            if (phase == 3){
                StartCoroutine(EnableAttackButton(14f));
            }
            else if (phase == 4){
                StartCoroutine(EnableAttackButton(6f));
            }
            else if (phase == 5){
                StartCoroutine(EnableAttackButton(8f));
            }
            else if (phase == 6){
                
            }
            else if (phase == 7){
                transform.position = new Vector2(transform.position.x, 0);
                StartCoroutine(Evolve());
            }
            else if (phase == 8){
                StartCoroutine(EnableAttackButton(4f));
            }
            else if (phase == 9){
                
            }
            else if (phase == 10){
                transform.position = new Vector2(transform.position.x, 0);
                animator.SetTrigger("Ulti");
                if (isLeft) Instantiate(chargeParticle, new Vector2(-8f, 0f), Quaternion.identity);
                else Instantiate(chargeParticle, new Vector2(13f, 2f), Quaternion.identity);
            }
            else if (phase == 11){
                
            }
        }
    }

    void OnDisable(){
        if (colorSlider != null)
            colorSlider.color = red;
    }

    IEnumerator EnableAttackButton(float sec)
    {
        yield return new WaitForSeconds(sec);
        buttonAttack.SetActive(true);
        yield return new WaitForSeconds(3f);
        buttonAttack.SetActive(false);
    }

    public void Attack(){
        if (gameObject.activeInHierarchy){
            if (LevelTwoValues.health < 3){
                LevelTwoValues.health++;
                healthSlider.value = LevelTwoValues.health;
            }
            animator.SetBool("Attack", true);
            buttonAttack.SetActive(false);
            colorSlider.color = healColor;
        }
    }

    public void ShowPowerParticle(){
        if (isLeft) Instantiate(powerParticle, new Vector2(-8f, 0f), Quaternion.identity);
        else Instantiate(powerParticle, new Vector2(13f, 2f), Quaternion.identity);
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

    public void StartSlowMo(){
        timeManager.StartSlowMotion();
    }

    public void StopSlowMo(){
        timeManager.StopSlowMotion();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponSoft") && !animator.GetBool("Attack"))
        {
            LevelTwoValues.health--;
            healthSlider.value = LevelTwoValues.health;
            if (LevelTwoValues.health <= 0){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    IEnumerator Evolve()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("Attack", false);
        animator.SetTrigger("Evolution");
    }
}
