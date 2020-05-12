using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OgreController : MonoBehaviour
{
    [Header("Health")]
    public int health = 0;
    public int initialHealth = 100;
    public GameObject healthBar;
    private Slider healthSlider;
    public int range = 6;
    public int damage = 20;

    [Header("Controllers")]
    public int speed = 10;

    [Header("Objects")]
    public GameObject target;
    public GameObject pre;
    public GameObject evolution;
    public GameObject quake;
    public Transform quakePoint;
    public ParticleSystem smoke;
    private float limit = 2.3f;
    private Animator ogreAnimation;
    private Vector3 initialPosition;
    private int RESTORE_HAMMER = 5, RESTORE_QUAKE = 5;
    private float distanceToTarget;
    private bool hammerCooldown = false, quakeCooldown = false, isAttacking = false, lookRight = false, freeze, isDead = false;

    void Start(){
        initialPosition = transform.position;
        healthSlider = healthBar.GetComponent<Slider>();
        ogreAnimation = GetComponent<Animator>();
        if (transform.localScale.x < 0)
            limit = -limit;
        quakePoint.Rotate(0f, 180f, 0f);
    }
    void OnEnable()
    {
        StartCoroutine(InitAttack());
    }

    void FixedUpdate()
    {
        healthSlider.value = health;

        if (!isDead){
            distanceToTarget = target.transform.position.x - transform.position.x;
            UpdateLook();
            FollowTarget();
        
            if (Mathf.Abs(distanceToTarget) < 2.5)
            {
                ogreAnimation.SetTrigger("Punch");
            }
            else if (Mathf.Abs(distanceToTarget) > range && !quakeCooldown)
            {
                StartCoroutine(QuakeAttack());
            }  
            else if (Mathf.Abs(distanceToTarget) > 4 && distanceToTarget < range && !hammerCooldown)
            {
                StartCoroutine(HammerAttack());
            }  
        }  
    }

    //Movement
    private void UpdateLook(){
        if (distanceToTarget > 0 && !lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
            limit = -limit;
            quakePoint.Rotate(0f, 180f, 0f);
        }
        else if (distanceToTarget < 0 && lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
            limit = -limit;
            quakePoint.Rotate(0f, 180f, 0f);
        }
    }

    private void FollowTarget(){
        if (Mathf.Abs(distanceToTarget) >= 2.3 && !freeze && !isAttacking)
        {
            ogreAnimation.SetBool("Run", true);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
    }

    //Attack
    IEnumerator HammerAttack()
    {
        hammerCooldown = true;
        isAttacking = true;
        ogreAnimation.SetTrigger("Hammer");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        yield return new WaitForSeconds(RESTORE_HAMMER);
        hammerCooldown = false;
    }

    IEnumerator QuakeAttack()
    {
        quakeCooldown = true;
        isAttacking = true;
        ogreAnimation.SetTrigger("Quake");
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        yield return new WaitForSeconds(RESTORE_QUAKE);
        quakeCooldown = false;
    }

    public void InvokeQuake(){
        Instantiate(quake, quakePoint.position, quakePoint.rotation);
    }

    //Collisions
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flash")
        {
            freeze = true;
            ogreAnimation.SetBool("Freeze", true);
            StartCoroutine(finishFreeze());
        }
        else if (collision.gameObject.tag == "WeaponMedium" && !isDead)
        {
            if (freeze)
                TakeDamage(damage);
            else
                TakeDamage(damage/4);
        }
        else if (collision.gameObject.tag == "PlayerDeath")
        {
            StartCoroutine(Respawn());
        }
    }

    //Auxiliar functions
    public void ShakeScreen(){
        CameraController.instance.Shake(0f);
    }

    private void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 50)
        {
            health = 50;
            ogreAnimation.SetTrigger("Die");
            StartCoroutine(Die());
        }
    }

    IEnumerator finishFreeze()
    {
        yield return new WaitForSeconds(5);
        freeze = false;
        ogreAnimation.SetBool("Freeze", false);
    }

    IEnumerator InitAttack()
    {
        health = initialHealth;
        healthBar.SetActive(true);
        freeze = true;
        yield return new WaitForSeconds(3);
        freeze = false;
    }

    IEnumerator Respawn()
    {
        ogreAnimation.SetBool("Idle", true);
        yield return new WaitForSeconds(2.1f);
        pre.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator Die()
    {
        isDead = true;
        healthBar.SetActive(false);
        yield return new WaitForSeconds(3);
        Instantiate(smoke, transform.position, transform.rotation);
        StartCoroutine(InvokeEvolution());
    }

    IEnumerator InvokeEvolution()
    {
        yield return new WaitForSeconds(5);
        evolution.transform.position = new Vector3(transform.position.x, transform.position.y, 0.63f);
        evolution.SetActive(true);
        gameObject.SetActive(false);
    }
}
