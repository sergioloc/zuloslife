using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody2D rb2d;
    public GameObject kero, panda, cinamon, kutter, triski;
    private Animator characterAnimation;
    public string currentCharacter; 

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

    public GameObject flashCollider;
    public ParticleSystem bloodParticle;

   



    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        characterAnimation = panda.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Movement
        if (joystick.Horizontal >= 0.2f)
        {
            characterAnimation.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!facingRight) Flip();
        }
        else if (joystick.Horizontal <= -0.2f)
        {
            characterAnimation.SetBool("Run", true);

            rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (facingRight) Flip();
        }
        else
        {
            characterAnimation.SetBool("Run", false);
        }

        //Jump
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            characterAnimation.SetBool("isJumping", false);
        }
        else
        {
            characterAnimation.SetBool("isJumping", true);
        }

        //Temp
        if (Input.GetKeyDown(KeyCode.G))
        {
            startAction();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            stopAction();
        }
    }

    public void Jump()
    {
        if (extraJumps > 0)
        {
            characterAnimation.SetTrigger("TakeOff");
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
        Debug.Log(isGrounded);
        characterAnimation.SetBool("Action", true);
        

        if (currentCharacter == "cinamon" && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jump * 80);
        }
        
    }

    public void stopAction()
    {
        characterAnimation.SetBool("Action", false);
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float forceX = 750;
        float forceY = 900;
        if (collision.gameObject.tag == "WeaponEnemy")
        {
            bloodParticle.Play();
            if (facingRight)
            {
                rb2d.AddForce(new Vector3(forceX, forceY));
            }
            else
            {
                rb2d.AddForce(new Vector3(forceX, forceY));
            }
        }
    }
    


    //Change character
    public void switchToPanda()
    {
        panda.SetActive(true);
        kero.SetActive(false);
        cinamon.SetActive(false);
        kutter.SetActive(false);
        triski.SetActive(false);
        characterAnimation = panda.GetComponent<Animator>();
        currentCharacter = "panda";
    }

    public void switchToKero()
    {
        panda.SetActive(false);
        kero.SetActive(true);
        cinamon.SetActive(false);
        kutter.SetActive(false);
        triski.SetActive(false);
        characterAnimation = kero.GetComponent<Animator>();
        currentCharacter = "kero";
    }

    public void switchToCinamon()
    {
        panda.SetActive(false);
        kero.SetActive(false);
        cinamon.SetActive(true);
        kutter.SetActive(false);
        triski.SetActive(false);
        characterAnimation = cinamon.GetComponent<Animator>();
        currentCharacter = "cinamon";
    }

    public void switchToKutter()
    {
        panda.SetActive(false);
        kero.SetActive(false);
        cinamon.SetActive(false);
        kutter.SetActive(true);
        triski.SetActive(false);
        characterAnimation = kutter.GetComponent<Animator>();
        currentCharacter = "kutter";
    }

    public void switchToTriski()
    {
        panda.SetActive(false);
        kero.SetActive(false);
        cinamon.SetActive(false);
        kutter.SetActive(false);
        triski.SetActive(true);
        characterAnimation = triski.GetComponent<Animator>();
        currentCharacter = "triski";
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

