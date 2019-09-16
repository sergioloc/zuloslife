using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OgreEvController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool moveLeft, moveRight;
    public float speed = 10;
    public GameObject lasers;
    public GameObject hits;
    private Animator ogreEvAnimation;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ogreEvAnimation = GetComponent<Animator>();
        moveLeft = false;
        moveRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ogreEvAnimation.SetBool("Eyes", true);
            StartCoroutine(LoadLaserAttack());
        }

        if (moveLeft)
        {
            MoveLeft();
        }

        if (moveRight)
        {
            MoveRight();
        }
    }

    private void LasserAttack()
    {
        transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        moveLeft = true;
        moveRight = false;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 4, transform.position.z), 5);
        lasers.SetActive(true);
        hits.SetActive(true);

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
        LasserAttack();
    }

}
