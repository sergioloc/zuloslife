using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireMissile : MonoBehaviour
{
    private bool check = false, isInArea = false;
    public GameObject projectile, laser;
    public Transform target, pos;

    void Start()
    {
    }

    void Update()
    {

        if (isInArea)
        {
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }

        Debug.Log(isInArea);
        if (check && isInArea)
        {
            check = false;
            StartCoroutine(ThrowMissile());
        }
    }

    IEnumerator ThrowMissile()
    {
        yield return new WaitForSeconds(2f);
        if (isInArea)
        {
            Fire();
            check = true;
        }
        
    }

    private void Fire()
    {
        Vector2 direction1 = target.position - transform.position;
        float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;
        Quaternion rotation1 = Quaternion.AngleAxis(angle1, Vector3.forward);
        Instantiate(projectile, new Vector3(pos.position.x, pos.position.y, 5f), rotation1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            check = true;
            isInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInArea = false;
            check = false;
        }
    }
}
