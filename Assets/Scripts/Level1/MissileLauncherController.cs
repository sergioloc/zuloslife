using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class MissileLauncherController : MonoBehaviour
{
    public GameObject projectile, laser;
    public Transform target, cannon, shotPoint;
    public float speedRotation = 50f;
    public bool reverse;
    private bool cooldown = false, isInArea = false, targetDead = false;

    void FixedUpdate()
    {
        if (isInArea && !targetDead)
        {
            laser.SetActive(true);
            RotateCannon();
            if (!cooldown)
            {
                cooldown = true;
                StartCoroutine(FireMissile());
            }
        }
        else
        {
            laser.SetActive(false);
        } 
    }

    private void RotateCannon(){
        Quaternion rotation;
        if (reverse)
            rotation = GetRotation(195f);
        else
            rotation = GetRotation(-15);
        cannon.rotation = Quaternion.Slerp(cannon.rotation, rotation, speedRotation * Time.deltaTime);
    }

    IEnumerator FireMissile()
    {
        yield return new WaitForSeconds(2f);
        if (isInArea)
        {  
            Instantiate(projectile, new Vector3(shotPoint.position.x, shotPoint.position.y, 0f), GetRotation(0f));
            cooldown = false;
        }
    }

    private Quaternion GetRotation(float offset){
        Vector2 direction = target.position - cannon.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle + offset, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cooldown = false;
            isInArea = true;
        }
        else if (collision.gameObject.CompareTag("PlayerDeath"))
        {
            StartCoroutine(Restart());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInArea = false;
            cooldown = true;
        }
    }

    IEnumerator Restart(){
        targetDead = true;
        yield return new WaitForSeconds(4f);
        targetDead = false;
    }
}
