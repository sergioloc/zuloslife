using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    public float jumpForce = 11f;
    public ParticleSystem electricPulse;

    void Start()
    {
        jumpForce = jumpForce * 1000;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            electricPulse.Play();
        } 
    }
}
