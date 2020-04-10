using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OgreController : MonoBehaviour
{
    public GameObject target, weapon, evolution, redLights, normalLights;
    public ParticleSystem smoke;
    public Slider healthBar;
    public int speed = 10;
    public int health = 100;
    public int range = 6;
    private Animator ogreAnimation;
    private bool lookRight = false, freeze;
    private float limit = 2.3f;
    private bool loadHammer = false, loadQuake = false, attacking = false;
    private bool idleAnim = false;
    private Vector3 initialPosition;

    private int RESTORE_ATTACK1 = 5;

    void Start()
    {
        initialPosition = transform.position;
        ogreAnimation = GetComponent<Animator>();
        if (transform.localScale.x < 0)
            limit = -limit;
        freeze = true;
        StartCoroutine(InitAttack());
    }

    void Update()
    {
        healthBar.value = health;

        if (health <= 0)
        {
            ogreAnimation.SetTrigger("Die");
            StartCoroutine(Die());
        }

        float distance = target.transform.position.x - transform.position.x;

        //Look
        if (distance > 0 && !lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
            limit = -limit;
        }
        else if (distance < 0 && lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
            limit = -limit;
        }

        if (!idleAnim)
        {
            ogreAnimation.SetBool("Idle", false);
            
            if (Mathf.Abs(distance) > range && !loadQuake)
            {
                //CameraController.instance.Shake(0.8f);
                loadQuake = true;
                attacking = true;
                ogreAnimation.SetTrigger("Quake");
                StartCoroutine(RestoreQuake());
                StartCoroutine(FinishAttacking());
            }
            else if (Mathf.Abs(distance) > 4 && distance < range && !loadHammer)
            {
                //CameraController.instance.Shake(0.7f);
                loadHammer = true;
                attacking = true;
                ogreAnimation.SetTrigger("Hammer");
                StartCoroutine(RestoreHammer());
                StartCoroutine(FinishAttacking());
            }
            else if (Mathf.Abs(distance) >= 2.3 && !freeze && !attacking)
            {
                ogreAnimation.SetBool("Run", true);
                ogreAnimation.SetTrigger("Quake");
                ogreAnimation.SetTrigger("Hammer");
                ogreAnimation.SetTrigger("Punch");
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
            if (Mathf.Abs(distance) < 2.5)
            {
                ogreAnimation.SetTrigger("Punch");
            }   
        }
        else
        {
            ogreAnimation.SetBool("Idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flash")
        {
            freeze = true;
            ogreAnimation.SetBool("Freeze", true);
            StartCoroutine(finishFreeze());
        }
        else if (collision.gameObject.tag == "Melee")
        {
            if (freeze)
                TakeDamage(10);
            else
                TakeDamage(1);
        }
        else if (collision.gameObject.tag == "PlayerDeath")
        {
            idleAnim = true;
            StartCoroutine(Restore());
        }

    }

    IEnumerator Restore()
    {
        yield return new WaitForSeconds(3);
        idleAnim = false;
        gameObject.transform.position = initialPosition;
        health = 100;
    }

    IEnumerator RestoreHammer()
    {
        yield return new WaitForSeconds(RESTORE_ATTACK1);
        loadHammer = false;
    }

    IEnumerator RestoreQuake()
    {
        yield return new WaitForSeconds(RESTORE_ATTACK1);
        loadQuake = false;
    }

    IEnumerator FinishAttacking()
    {
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    private void TakeDamage(int damage)
    {
        health = health - damage;
    }

    IEnumerator finishFreeze()
    {
        yield return new WaitForSeconds(5);
        freeze = false;
        ogreAnimation.SetBool("Freeze", false);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        smoke.transform.position = new Vector3(transform.position.x, smoke.transform.position.y, smoke.transform.position.z);
        if (!smoke.isPlaying)
        {
            smoke.Play();
        }
        else
        {
            //CameraController.instance.Shake(0f);
        }
        StartCoroutine(InvokeEvolution());
    }

    IEnumerator InvokeEvolution()
    {
        yield return new WaitForSeconds(5);
        normalLights.SetActive(false);
        redLights.SetActive(true);
        evolution.transform.position = new Vector3(transform.position.x, transform.position.y, 0.63f);
        evolution.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator InitAttack()
    {
        yield return new WaitForSeconds(3);
        freeze = false;
    }
}
