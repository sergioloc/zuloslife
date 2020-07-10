using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class Scissor : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject destroyEffect;

    void Awake(){
        speed = ConfigManager.appConfig.GetInt("scissorSpeed");
        if (speed == 0) speed = 25;
    }

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
