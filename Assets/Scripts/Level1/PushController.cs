using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    private bool facingRight;
    public bool front;
    public int strong, high;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponSoft") || collision.gameObject.CompareTag("WeaponMedium"))
        {
            Push(strong);
        }
        else if (collision.gameObject.CompareTag("WeaponHard"))
        {
            Push(strong*20);
        }
    }

    private void Push(int mStrong)
    {
        if (front)
        {
            if (PlayerController.instance.facingRight)
            {
                rigidbody2d.AddForce(new Vector3(-mStrong, high));
            }
            else
            {
                rigidbody2d.AddForce(new Vector3(mStrong, high));
            }
        }
        else
        {
            if (PlayerController.instance.facingRight)
            {
                rigidbody2d.AddForce(new Vector3(mStrong, high));
            }
            else
            {
                rigidbody2d.AddForce(new Vector3(-mStrong, high));
            }
        }
    }
}