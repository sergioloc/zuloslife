using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage;
    public float speed;
    public GameObject particle;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            collision.GetComponent<TricycleController>().health -= damage;
            Destroy(gameObject);
        }
    }

}
