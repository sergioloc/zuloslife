using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage;
    public float speed = 5;
    public GameObject particle;
    private float time = 0;

    void Start()
    {
        time = Time.realtimeSinceStartup;
    }
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.M))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            TrycicleLevelValues.health -= damage;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Shield")){
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
