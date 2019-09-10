using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OgreController : MonoBehaviour
{
    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;
    private float shakeElapsedTime = 0f;

    public GameObject target, weapon, initialPosition, evolution;
    public Slider healthBar;
    public int speed = 10;
    public int health = 100;
    public int range = 6;
    private Animator ogreAnimation;
    private bool lookRight = false, freeze = false;
    private float limit = 2.3f;
    private bool loadAttack1 = false, loadAttack2 = false, attacking1 = false, attacking2 = false;
    private bool idleAnim = false;

    private int RESTORE_ATTACK1 = 5;

    void Start()
    {
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        ogreAnimation = GetComponent<Animator>();
        if (transform.localScale.x < 0)
            limit = -limit;
    }

    void Update()
    {
        healthBar.value = health;
        ShakeCamera();

        if (target != null)
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

            //Debug.Log(distance);
            if (!idleAnim)
            {
                if (distance > range && distance < 22 && !loadAttack2)
                {
                    StartCoroutine(Wait(0.8f));
                    ogreAnimation.SetBool("Action2", true);
                    loadAttack2 = true;
                    attacking2 = true;
                    StartCoroutine(RestoreAttack2());
                    StartCoroutine(FinishAttack2());
                }

                if (distance < range && distance > (range - 1) && !loadAttack1)
                {
                    StartCoroutine(Wait(0.7f));
                    ogreAnimation.SetBool("Run", false);
                    ogreAnimation.SetBool("Action", true);
                    ogreAnimation.SetBool("Action2", false);
                    ogreAnimation.SetBool("Action3", false);
                    loadAttack1 = true;
                    attacking1 = true;
                    StartCoroutine(RestoreAttack1());
                    StartCoroutine(FinishAttack1());
                }

                else if (distance >= 3.5 && !freeze && !attacking1 && !attacking2)
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
            evolution.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            evolution.SetActive(true);
            gameObject.SetActive(false);
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
        else if (collision.gameObject.tag == "Weapon" && !freeze)
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.tag == "Weapon" && freeze)
        {
            TakeDamage(5);
        }
        else if (collision.gameObject.tag == "PlayerDeath")
        {
            idleAnim = true;
            StartCoroutine(Restore());
        }

    }

    IEnumerator Restore()
    {
        yield return new WaitForSeconds(4);
        idleAnim = false;
        gameObject.transform.position = initialPosition.transform.position;
        health = 100;
    }

    IEnumerator RestoreAttack1()
    {
        yield return new WaitForSeconds(RESTORE_ATTACK1);
        loadAttack1 = false;
    }

    IEnumerator RestoreAttack2()
    {
        yield return new WaitForSeconds(RESTORE_ATTACK1);
        loadAttack2 = false;
    }

    IEnumerator FinishAttack1()
    {
        yield return new WaitForSeconds(2);
        attacking1 = false;
    }

    IEnumerator FinishAttack2()
    {
        yield return new WaitForSeconds(3);
        attacking2 = false;
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

    IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
        shakeElapsedTime = shakeDuration;
    }

    private void ShakeCamera()
    {
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            if (shakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }
    }
}
