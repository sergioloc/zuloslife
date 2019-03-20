using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody2D rb2d;

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
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        Debug.Log("isGrounded: "+isGrounded);

        //Movement
        if (joystick.Horizontal >= 0.2f)
        {
            rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!facingRight) Flip();
        }
        if(joystick.Horizontal <= -0.2f)
        {
            rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (facingRight) Flip();
        }
        //Jump

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (joystick.Vertical >= 0.5f && extraJumps > 0)
        {
            rb2d.AddForce(Vector2.up * jump);
            extraJumps--;
            Debug.Log("Jump");
        }
        else if (joystick.Vertical >= 0.5f && extraJumps == 0 && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jump);
            Debug.Log("Last Jump");
        }
        
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        if(facingRight)
            transform.localScale = new Vector3(-scale, scale, scale);
        else
            transform.localScale = new Vector3(scale, scale, scale);
        facingRight = !facingRight;
    }
}
