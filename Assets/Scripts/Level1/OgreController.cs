﻿using Cinemachine;
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

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip dieSound;
    public AudioClip[] attackSounds;

    [Header("Objects")]
    private GameObject target;
    public GameObject chief;
    public GameObject spectro;
    public GameObject quake;
    public Transform quakePoint;
    public ParticleSystem smoke;
    private float limit = 2.3f;
    private Animator ogreAnimation;
    private int RESTORE_HAMMER = 5, RESTORE_QUAKE = 5;
    private float distanceToTarget;
    private bool hammerCooldown = false, quakeCooldown = false, isAttacking = false, lookRight = false, freeze, isAlive;

    void Start(){
        target = GameObject.Find("Player");
        healthSlider = healthBar.GetComponent<Slider>();
        ogreAnimation = GetComponent<Animator>();
    }

    void OnEnable(){
        isAlive = true;
        gameObject.transform.position = new Vector3(20f, -63f, 0f);
        if (transform.localScale.x < 0)
            limit = -Mathf.Abs(limit);
        quakeCooldown = false;
        hammerCooldown = false;
        StartCoroutine(InitAttack());
    }

    void Update()
    {
        healthSlider.value = health;

        if (LevelOneValues.isPlayerAlive){
            if (isAlive && !freeze){
                distanceToTarget = target.transform.position.x - transform.position.x;
                LookAtTarget();
                FollowTarget();

                if (Mathf.Abs(distanceToTarget) < 2.5)
                {
                    ogreAnimation.SetTrigger("Punch");
                }
                else if (Mathf.Abs(distanceToTarget) > range && !quakeCooldown)
                {
                    StartCoroutine(QuakeAttack());
                }  
                else if (Mathf.Abs(distanceToTarget) > 4 && Mathf.Abs(distanceToTarget) < range && !hammerCooldown)
                {
                    StartCoroutine(HammerAttack());
                }
            }
        }
        else{
            StartCoroutine(Respawn());
        }
    }

    //Movement
    private void LookAtTarget(){
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
        if (Mathf.Abs(distanceToTarget) >= 2.3 && !isAttacking)
        {
            ogreAnimation.SetBool("Run", true);
            Vector3 position = new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        }
        else {
            ogreAnimation.SetBool("Run", false);
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

    public void PlaySoundAttack(){
        if (!audioSource.isPlaying && isAlive){
            int rand = new System.Random().Next(0, attackSounds.Length);
            audioSource.PlayOneShot(attackSounds[rand]);
        }
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
        else if (collision.gameObject.tag == "WeaponMedium" && isAlive)
        {
            if (freeze)
                TakeDamage(damage);
            else
                TakeDamage(damage/4);
        }
    }

    //Auxiliar functions
    public void ShakeScreen(){
        CinemachineController.instance.Shake(0f);
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
        yield return new WaitForSeconds(2);
        freeze = false;
    }

    IEnumerator Respawn()
    {
        ogreAnimation.SetBool("Run", false);
        yield return new WaitForSeconds(2.1f);
        chief.SetActive(true);
        healthBar.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator Die()
    {
        isAlive = false;
        yield return new WaitForSeconds(3);
        audioSource.volume = 1f;
        audioSource.PlayOneShot(dieSound);
        Instantiate(smoke, transform.position, transform.rotation);
        StartCoroutine(InvokeEvolution());
    }

    IEnumerator InvokeEvolution()
    {
        yield return new WaitForSeconds(5);
        spectro.transform.position = new Vector3(transform.position.x, transform.position.y, 0.63f);
        spectro.SetActive(true);
        gameObject.SetActive(false);
    }
}
