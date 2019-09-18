using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OgreEvController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool moveLeft, moveRight, followPlayer, lookRight, laserAttack, freeze;
    public float speed = 50;
    public GameObject lasers, hits, target, level2, jumpPad;
    private Animator ogreEvAnimation;
    private float limit = 5f;
    public ParticleSystem lava;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ogreEvAnimation = GetComponent<Animator>();
        moveLeft = false;
        moveRight = false;
        followPlayer = false;
        lookRight = false;
        laserAttack = false;
        freeze = false;
        StartMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!laserAttack)
            {
                ogreEvAnimation.SetBool("Eyes", true);
                StopMovement();
                StartCoroutine(LoadLaserAttack());
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (laserAttack)
                StopLasserAttack();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            StopMovement();
            followPlayer = true;
            ogreEvAnimation.SetTrigger("Fist");
            StartCoroutine(StopFist());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ogreEvAnimation.SetTrigger("Mouth");
            StartCoroutine(ThrowLava());
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
            float distance = Vector2.Distance(transform.position, target.transform.position);
            float x1 = target.transform.position.x;
            float x2 = transform.position.x;
            float realDistance = x1 - x2;

            //Look
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

            if (realDistance > 7 || realDistance < -7)
            {
                if (lookRight)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), 30 * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x + limit, transform.position.y, transform.position.z), 30 * Time.deltaTime);
            }

        }
    }
        

    private void StartLasserAttack()
    {
        laserAttack = true;
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

    private void StopLasserAttack()
    {
        laserAttack = false;
        ogreEvAnimation.SetBool("Eyes", false);
        lasers.SetActive(false);
        hits.SetActive(false);
        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
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
            freeze = true;
            StartCoroutine(UnFreeze());
        }

        if (collision.gameObject.tag == "Melee")
        {
            level2.SetActive(true);
            jumpPad.SetActive(false);
            Destroy(gameObject);
        }
    }

    IEnumerator LoadLaserAttack()
    {
        yield return new WaitForSeconds(1.5f);
        StartMovement();
        StartLasserAttack();
    }

    IEnumerator ThrowLava()
    {
        yield return new WaitForSeconds(1.2f);
        lava.Play();
    }

    IEnumerator StopFist()
    {
        yield return new WaitForSeconds(2f);
        if (!freeze)
            StartMovement();
        followPlayer = false;
    }

    IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(2f);
        freeze = false;
        StartCoroutine(StopFist());
    }

}
