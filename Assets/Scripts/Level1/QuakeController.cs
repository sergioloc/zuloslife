using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeController : MonoBehaviour
{
    public int speed = 0;
    public GameObject explosion;
    public ParticleSystem particle;
    private bool destroyed = false;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!destroyed)
            rb2d.velocity = transform.right * speed * 100 * Time.deltaTime;
        else
            rb2d.velocity = new Vector2(0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            destroyed = true;
            particle.Stop();
            StartCoroutine(DestroyQuake());
        }
        else if (collision.gameObject.CompareTag("WallRight") || collision.gameObject.CompareTag("WallLeft"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            destroyed = true;
            particle.Stop();
            StartCoroutine(DestroyQuake());
        }
    }

    IEnumerator DestroyQuake(){
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
