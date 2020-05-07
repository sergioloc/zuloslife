using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    public float jumpForce = 11f;
    public ParticleSystem electricPulse;
    private Rigidbody2D target;

    void Start()
    {
        jumpForce = jumpForce * 1000;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Jumpad");
            target = collision.gameObject.GetComponent<Rigidbody2D>();
            target.drag = 0f;
            target.velocity = new Vector3(0f, 0f, 0f);
            target.AddForce(new Vector2(0f, jumpForce));
            electricPulse.Play();
        } 
    }
}
