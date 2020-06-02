using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierogi : MonoBehaviour
{
    public float speed = 10f;
    public GameObject explosion;
    private Rigidbody2D rb2d;
    public SpriteRenderer sprite;
    private Collider2D collider2d;
    private AudioSource audioSource;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        rb2d.velocity = transform.right * speed * 100 * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("WeaponSoft")
            || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Shield"))
        {
            audioSource.Play();
            Instantiate(explosion, transform.position, Quaternion.identity);
            speed = 0;
            sprite.enabled = false;
            collider2d.enabled = false;
            Destroy(gameObject, 2f);
        }
    }

}
