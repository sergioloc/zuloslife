using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Cinemachine;

public class OgreEvController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool moveLeft, moveRight, followPlayer, lookRight, waiting, freeze;
    public float speed = 50;
    public GameObject lasers, lasersPoints, target, level2, jumpPad, door, wallRight, floor6, healthBar, hitBox;
    private Animator ogreEvAnimation;
    private float limit = 5f, distance;
    public ParticleSystem lava, implosion, death;
    public int health = 100, initialHealth = 1;
    private Slider healthSlider;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = new Vector3(0f,0f,0f);
        healthSlider = healthBar.GetComponent<Slider>();
        rb2d = GetComponent<Rigidbody2D>();
        ogreEvAnimation = GetComponent<Animator>();
        moveLeft = false;
        moveRight = false;
        followPlayer = false;
        lookRight = false;
        waiting = false;
        freeze = true;
        UpdateLook();
        StartCoroutine(InitMovement());
    }

    void FixedUpdate()
    {
        healthSlider.value = health;

        if (!waiting && !freeze)
        {
            waiting = true;
            System.Random rnd = new System.Random();
            int i = rnd.Next(0, 3);
            if (i == 0)
            {
                PrepareLaser();
            }
            else if (i == 1)
            {
                FistAttack();
            }
            else if (i == 2)
            {
                LavaAttack();
            }
        }

        if (moveLeft && !freeze)
        {
            MoveLeft();
        }
        else if (moveRight && !freeze)
        {
            MoveRight();
        }
        else if (followPlayer)
        {
            UpdateLook();
            
            if (Mathf.Abs(distance) > 7)
            {
                if (lookRight)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), 30 * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x + limit, transform.position.y, transform.position.z), 30 * Time.deltaTime);
            }
            
        }
    }

    private void UpdateLook()
    {
        distance = target.transform.position.x - transform.position.x;

        if (distance > 0 && lookRight)
        {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (distance < 0 && !lookRight)
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (distance > 0 && !lookRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
        }
        else if (distance < 0 && lookRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
        }
    }
        
    // Movement
    private void MoveLeft()
    {
        rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
        lookRight = false;
    }

    private void MoveRight()
    {
        rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
        lookRight = true;
    }

    private void StartMovement()
    {
        transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        moveLeft = true;
        moveRight = false; 
    }

    private void StopMovement()
    {
        moveLeft = false;
        moveRight = false;
    }

    // Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!followPlayer)
        {
            if (collision.gameObject.tag == "WallLeft")
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                moveLeft = false;
                moveRight = true;
            }

            if (collision.gameObject.tag == "WallRight")
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                moveLeft = true;
                moveRight = false;
            }
        }
        
        if (collision.gameObject.tag == "Sparks")
        {
            health = health - 20;
            freeze = true;
            StartCoroutine(UnFreeze());
            if (health <= 0)
                Die();
        }

        if (collision.gameObject.tag == "Projectile")
        {
            health = health - 10;
            if (health <= 0)
                Die();
        }

        if (collision.gameObject.tag == "PlayerDeath")
        {
            freeze = true;
            StartCoroutine(Restore());
        }
    }

    IEnumerator InitMovement()
    {
        yield return new WaitForSeconds(3.5f);
        StartMovement();
        freeze = false;
    }

    IEnumerator Restore()
    {
        yield return new WaitForSeconds(3f);
        health = initialHealth; 
        transform.position = initialPosition;
        freeze = false;
    }

    //Attacks
    private void PrepareLaser()
    {
        ogreEvAnimation.SetBool("Eyes", true);
        StopMovement();
        StartCoroutine(Laser());
    }

    IEnumerator Laser()
    {
        yield return new WaitForSeconds(1.5f);
        StartMovement();
        hitBox.SetActive(true);
        lasers.SetActive(true);
        lasersPoints.SetActive(true);
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        StartCoroutine(StopLaser());
    }

    IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(6f);
        ogreEvAnimation.SetBool("Eyes", false);
        hitBox.SetActive(false);
        lasers.SetActive(false);
        lasersPoints.SetActive(false);
        if (!freeze)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
            StartCoroutine(Wait());
        }
    }

    private void FistAttack()
    {
        StopMovement();
        followPlayer = true;
        ogreEvAnimation.SetTrigger("Fist");
        StartCoroutine(StopFist());
    }

    private void LavaAttack()
    {
        StopMovement();
        followPlayer = true;
        ogreEvAnimation.SetTrigger("Mouth");
        StartCoroutine(ThrowLava());
    }

    

    IEnumerator ThrowLava()
    {
        yield return new WaitForSeconds(1.2f);
        lava.Play();
        StartCoroutine(StopFist());
    }

    IEnumerator StopFist()
    {
        yield return new WaitForSeconds(2f);
        if (!freeze)
            StartMovement();
        followPlayer = false;
        StartCoroutine(Wait());
    }

    

    IEnumerator Wait()
    {
        System.Random rnd = new System.Random();
        int i = rnd.Next(3, 6);
        yield return new WaitForSeconds(i);
        waiting = false;
    }


    IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(2f);
        freeze = false;
        StartCoroutine(StopFist());
    }

    private void Die()
    {
        freeze = true;
        implosion.transform.position = new Vector3(transform.position.x, transform.position.y, implosion.transform.position.z);
        implosion.Play();
        death.transform.position = new Vector3(transform.position.x, transform.position.y, death.transform.position.z);
        death.Play();
        Destroy(gameObject, 2f);
        healthBar.SetActive(false);
        OpenLevel2();
    } 

    private void OpenLevel2()
    {
        level2.SetActive(true);
        jumpPad.SetActive(false);
        door.SetActive(false);
        wallRight.SetActive(false);
        floor6.SetActive(true);
    }
}
