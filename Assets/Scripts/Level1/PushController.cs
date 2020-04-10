using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    private bool facingRight;
    public bool front;
    public int strong, high;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MeleeEnemy") || collision.gameObject.CompareTag("OgreFist") || collision.gameObject.CompareTag("OgreQuake"))
        {
            Push();
        }
    }

    private void Push()
    {
        if (front)
        {
            if (PlayerController.instance.facingRight)
            {
                rigidbody2d.AddForce(new Vector3(-strong, high));
            }
            else
            {
                rigidbody2d.AddForce(new Vector3(strong, high));
            }
        }
        else
        {
            if (PlayerController.instance.facingRight)
            {
                rigidbody2d.AddForce(new Vector3(strong, high));
            }
            else
            {
                rigidbody2d.AddForce(new Vector3(-strong, high));
            }
        }
    }
}