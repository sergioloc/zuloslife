using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OgreEvController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool moveLeft, moveRight, followPlayer, lookRight;
    public float speed = 10;
    public GameObject lasers, hits, target;
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
        followPlayer = true;
        lookRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ogreEvAnimation.SetBool("Eyes", true);
            followPlayer = false;
            StartCoroutine(LoadLaserAttack());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            StopLasserAttack();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (lava.isPlaying)
                lava.Stop();
            else
                lava.Play();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ogreEvAnimation.SetTrigger("Fist");
        }

        if (moveLeft)
        {
            MoveLeft();
        }

        if (moveRight)
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

            if (distance > 7)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }

            //Debug.Log(distance);
        }
    }
        

    private void StartLasserAttack()
    {
        transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        moveLeft = true;
        moveRight = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        lasers.SetActive(true);
        hits.SetActive(true);
    }

    private void StopLasserAttack()
    {
        lookRight = false;
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        moveLeft = false;
        moveRight = false;
        followPlayer = true;
        ogreEvAnimation.SetBool("Eyes", false);
        lasers.SetActive(false);
        hits.SetActive(false);
        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, transform.position.z), 5);
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

    IEnumerator LoadLaserAttack()
    {
        yield return new WaitForSeconds(2);
        StartLasserAttack();
    }

}
