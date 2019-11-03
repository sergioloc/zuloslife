using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireMissile : MonoBehaviour
{
    private bool check = true;
    public GameObject projectile, laser;
    public Transform target, pos;
    public float distanceLimitX = 0;
    public float distanceLimitY = 0;
    private float distanceX, distanceY;

    void Start()
    {

    }

    void Update()
    {
        distanceX = target.transform.position.x - transform.position.x;
        distanceY = target.transform.position.y - transform.position.y;

        if (Math.Abs(distanceX) < distanceLimitX && Math.Abs(distanceY) < distanceLimitY)
        {
            laser.SetActive(true);
        }
        else
        {
            laser.SetActive(false);
        }

        //Debug.Log(distance);
        if (check && Math.Abs(distanceX) < distanceLimitX && Math.Abs(distanceY) < distanceLimitY)
        {
            check = false;
            StartCoroutine(ThrowMissile());
        }
    }

    IEnumerator ThrowMissile()
    {
        yield return new WaitForSeconds(2f);
        Fire();
        check = true;
    }

    private void Fire()
    {
        //Rotate launcher
        Vector2 direction1 = target.position - transform.position;
        float angle1 = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg;
        Quaternion rotation1 = Quaternion.AngleAxis(angle1, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation1, 5 * Time.deltaTime);
        
        //Init missile
        Instantiate(projectile, new Vector3(pos.position.x, pos.position.y, 5f), rotation1);
    }
}
