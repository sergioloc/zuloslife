using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RotateCannon : MonoBehaviour
{
    public float speed = 50f;
    public Transform target;
        public bool reverse;
    private float angle;

    void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (reverse)
            angle = angle + 195f;
        else
            angle = angle - 15f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }
}
