using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OgreController : MonoBehaviour
{
    public GameObject target, weapon, initialPosition, evolution, redLights, normalLights;
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

    private int RESTORE_ATTACK1 = 5;

    void Start()
    {
        ogreAnimation = GetComponent<Animator>();
        if (transform.localScale.x < 0)
            limit = -limit;
        freeze = true;
        StartCoroutine(InitAttack());
    }

    void Update()
    {
        healthBar.value = health;

        if (target != null && health > 0)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            float x1 = target.transform.position.x;
            float x2 = transform.position.x;
            float realDistance = x1 - x2;

            //Look
            if (realDistance > 0 && !lookRight && !freeze)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                lookRight = true;
                limit = -limit;
            }
            else if (realDistance < 0 && lookRight && !freeze)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                lookRight = false;
                limit = -limit;
            }

            if (!idleAnim)
            {
                if (distance > range && !loadQuake)
                {
                    //CameraController.instance.Shake(0.8f);
                    ogreAnimation.SetBool("Action2", true);
                    loadQuake = true;
                    attacking = true;
                    StartCoroutine(RestoreQuake());
                    StartCoroutine(FinishAttacking());
                }
                else if (distance < range && distance > 4 && !loadHammer)
                {
                    //CameraController.instance.Shake(0.7f);
                    ogreAnimation.SetBool("Action", true);
                    loadHammer = true;
                    attacking = true;
                    StartCoroutine(RestoreHammer());
                    StartCoroutine(FinishAttacking());
                }

                else if (distance >= 3.5 && !freeze && !attacking)
                {
                    ogreAnimation.SetBool("Run", true);
                    ogreAnimation.SetBool("Action", false);
                    ogreAnimation.SetBool("Action2", false);
                    ogreAnimation.SetBool("Action3", false);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
                }

                if (distance < 3.5)
                {
                    ogreAnimation.SetBool("Action3", true);
                }
            }
            else
            {
                ogreAnimation.SetBool("Run", false);
                ogreAnimation.SetBool("Action", false);
                ogreAnimation.SetBool("Action2", false);
                ogreAnimation.SetBool("Action3", false);
            }
        }

        if (health <= 0)
        {
            ogreAnimation.SetBool("Die", true);
            StartCoroutine(Die());
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
        else if (collision.gameObject.tag == "Melee" && !freeze)
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.tag == "Melee" && freeze)
        {
            TakeDamage(10);
        }
        else if (collision.gameObject.tag == "PlayerDeath")
        {
            loadQuake = false;
            loadHammer = false;
            attacking = false;
        }

    }

    IEnumerator Restore()
    {
        yield return new WaitForSeconds(4);
        idleAnim = false;
        Debug.Log("Restore");
        gameObject.transform.position = initialPosition.transform.position;
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
        yield return new WaitForSeconds(3);
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
            CameraController.instance.Shake(0f);
        }
        StartCoroutine(InvokeEvolution());
    }

    IEnumerator InvokeEvolution()
    {
        yield return new WaitForSeconds(5);
        normalLights.SetActive(false);
        redLights.SetActive(true);
        evolution.transform.position = new Vector3(transform.position.x, transform.position.y, -69);
        evolution.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator InitAttack()
    {
        yield return new WaitForSeconds(3);
        freeze = false;
    }
}
