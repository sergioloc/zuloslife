using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public int damage;
    private float speed = 5;
    public GameObject particle;
    private float time = 0;
    public int phase2, phase3, phase4;

    void Start()
    {
        time = Time.realtimeSinceStartup;
        if (time > phase2)
            speed = 7;
        if (time > phase3)
            speed = 10;
        if (time > phase4)
            speed = 15;
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
            collision.GetComponent<TricycleController>().health -= damage;
            Destroy(gameObject);
        }
    }

}
