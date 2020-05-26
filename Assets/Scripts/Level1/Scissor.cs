using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissor : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject destroyEffect;

    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lab")
        {
            Destroy(collision.gameObject);
            DestroyProjectile();
        }
        else if (collision.gameObject.tag == "Enemy"  || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "WallRight" || collision.gameObject.tag == "WallLeft"){
            DestroyProjectile();
        }
    }
    

    void DestroyProjectile()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
