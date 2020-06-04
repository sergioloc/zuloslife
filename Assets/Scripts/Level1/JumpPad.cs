using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    public float jumpForce = 11f;
    public ParticleSystem electricPulse;
    private Rigidbody2D target;
    private AudioSource audioSource;

    void Start()
    {
        jumpForce = jumpForce * 1000;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.gameObject.GetComponent<Rigidbody2D>();
            target.drag = 0f;
            target.velocity = new Vector3(0f, 0f, 0f);
            target.AddForce(new Vector2(0f, jumpForce));
            electricPulse.Play();
            audioSource.Play();
        } 
    }
}
