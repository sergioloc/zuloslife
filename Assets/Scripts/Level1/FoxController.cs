using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    public float speed = 0.1f;
    private float rotateSpeed = 5.0f;
    public int range = 5;
    public float alpha = 100f;
    public ParticleSystem explosion;
    public GameObject target, foxSprite;
    private Vector3 newPosition;
    private float distanceX, distanceY, angle;

    void Start()
    {
        PositionChange();
        Color tmp = foxSprite.GetComponent<SpriteRenderer>().color;
        tmp.a = alpha;
        foxSprite.GetComponent<SpriteRenderer>().color = tmp;
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance < range)
        {
            LookAt2D(target.gameObject.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, Time.deltaTime * speed * 10f);
        }
        else
        {
            if (Vector2.Distance(transform.position, newPosition) < 1)
                PositionChange();

            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * speed);

            LookAt2D(newPosition);
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            Exploit();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Exploit();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Exploit();
    }

    void PositionChange()
    {
        newPosition = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    void LookAt2D(Vector3 lookAtPosition)
    {
        distanceX = lookAtPosition.x - transform.position.x;
        distanceY = lookAtPosition.y - transform.position.y;
        angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;

        Quaternion endRotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.deltaTime * rotateSpeed);
    }

    void Exploit()
    {
        explosion.Play();
        foxSprite.SetActive(false);
        Destroy(gameObject, 2);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
