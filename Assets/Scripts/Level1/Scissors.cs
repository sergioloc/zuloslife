using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissors : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject destroyEffect;

    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Guard"  || collision.gameObject.tag == "WallRight" || collision.gameObject.tag == "WallLeft" || collision.gameObject.tag == "Enemy")
        {
            DestroyProjectile();
        }
    }
    

    void DestroyProjectile()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
