using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage;
    public float speed;
    public GameObject particle;

    void Start()
    {
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<TricycleController>().health -= damage;
            Debug.Log(collision.GetComponent<TricycleController>().health);
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
