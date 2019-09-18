using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OgreEvController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool moveLeft, moveRight, followPlayer, lookRight, waiting, freeze;
    public float speed = 50;
    public GameObject lasers, hits, target, level2, jumpPad, door, wallRight;
    private Animator ogreEvAnimation;
    private float limit = 5f, realDistance;
    public ParticleSystem lava;
    public int health = 100;
    public Slider healthBar;


    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;

        if (!waiting && !freeze)
        {
            waiting = true;
            System.Random rnd = new System.Random();
            int i = rnd.Next(0, 3);
            Debug.Log(i);
            if (i == 0)
            {
                LaserAttack();
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

        if (moveRight && !freeze)
        {
            MoveRight();
        }
        
        if (followPlayer)
        {
            UpdateLook();

            if (realDistance > 7 || realDistance < -7)
            {
                if (lookRight)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), 30 * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x + limit, transform.position.y, transform.position.z), 30 * Time.deltaTime);
            }
        }

        if (health <= 0)
        {
            Die();
            OpenLevel2();
        }
    }

    private void UpdateLook()
    {
        realDistance = target.transform.position.x - transform.position.x;
        if (realDistance > 0 && !lookRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
        }
        else if (realDistance < 0 && lookRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
        }
    }
        

    private void StartLasserAttack()
    {
        lasers.SetActive(true);
        hits.SetActive(true);
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
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

    

    private void MoveLeft()
    {
        rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void MoveRight()
    {
        rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

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
        }

        if (collision.gameObject.tag == "Projectile")
        {
            health = health - 10;
        }
    }

    

    IEnumerator InitMovement()
    {
        yield return new WaitForSeconds(3.5f);
        StartMovement();
        freeze = false;
    }

    //Attacks

    private void LaserAttack()
    {
        ogreEvAnimation.SetBool("Eyes", true);
        StopMovement();
        StartCoroutine(LoadLaserAttack());
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

    IEnumerator LoadLaserAttack()
    {
        yield return new WaitForSeconds(1.5f);
        StartMovement();
        StartLasserAttack();
        StartCoroutine(StopLasserAttack());
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

    IEnumerator StopLasserAttack()
    {
        yield return new WaitForSeconds(6f);
        ogreEvAnimation.SetBool("Eyes", false);
        lasers.SetActive(false);
        hits.SetActive(false);
        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        System.Random rnd = new System.Random();
        int i = rnd.Next(3, 6);
        Debug.Log("Wait "+i);
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
        Destroy(gameObject);
    }

    private void OpenLevel2()
    {
        level2.SetActive(true);
        jumpPad.SetActive(false);
        door.SetActive(false);
        wallRight.SetActive(false);
    }
}
