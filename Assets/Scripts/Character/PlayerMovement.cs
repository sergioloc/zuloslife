using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody2D rb2d;

    public int velocity = 0;
    private bool facingRight = true;
    private float scale;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(joystick.Horizontal >= 0.2f)
        {
            rb2d.AddForce(new Vector2(velocity, 0));
            if (!facingRight) Flip();
        }
        if(joystick.Horizontal <= -0.2f)
        {
            rb2d.AddForce(new Vector2(-velocity, 0));
            if (facingRight) Flip();
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
