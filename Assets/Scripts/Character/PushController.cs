using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    private bool facingRight;
    public bool front;
    public int strong, high;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        facingRight = PlayerController.instance.facingRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MeleeEnemy"))
        {
            Push();
        }
    }

    private void Push()
    {

        if (front)
        {
            if (facingRight)
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
            if (facingRight)
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