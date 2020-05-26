using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public GameObject destroyEffect;
    public string[] destroyTag;

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < destroyTag.Length; i++){
            if (collision.gameObject.tag == destroyTag[i])
            {
                if (destroyEffect != null)
                    Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        
    }
}
