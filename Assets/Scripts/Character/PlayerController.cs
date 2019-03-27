using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody2D rb2d;
    public GameObject kero, panda;
    private Animator charcterAnimation;

    public int speed = 0;
    public int jump = 0;
    private bool facingRight = true;
    private float scale;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;



    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        charcterAnimation = kero.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Movement
        if (joystick.Horizontal >= 0.2f)
        {
            charcterAnimation.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!facingRight) Flip();
        }
        else if (joystick.Horizontal <= -0.2f)
        {
            charcterAnimation.SetBool("Run", true);

            rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (facingRight) Flip();
        }
        else
        {
            charcterAnimation.SetBool("Run", false);
        }

        //Jump
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            charcterAnimation.SetBool("isJumping", false);
        }
        else
        {
            charcterAnimation.SetBool("isJumping", true);
        }
    }

    public void Jump()
    {
        if (extraJumps > 0)
        {
            charcterAnimation.SetTrigger("TakeOff");
            rb2d.AddForce(Vector2.up * jump * 100);
            extraJumps--;
        }
        else if (extraJumps == 0 && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jump);
        }
    }

    //Action
    public void startAction()
    {
        charcterAnimation.SetBool("Action", true);
    }

    public void stopAction()
    {
        charcterAnimation.SetBool("Action", false);

    }

    //Change character
    public void switchToPanda()
    {
        panda.SetActive(true);
        kero.SetActive(false);
    }

    public void switchToKero()
    {
        panda.SetActive(false);
        kero.SetActive(true);
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        if (facingRight)
            transform.localScale = new Vector3(-scale, scale, scale);
        else
            transform.localScale = new Vector3(scale, scale, scale);
        facingRight = !facingRight;
    }
}

