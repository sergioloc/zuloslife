using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RotateLauncher : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    private float extra, distanceX, distanceY, angle;
    public bool reverse;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distanceX = target.transform.position.x - transform.position.x;
        distanceY = target.transform.position.y - transform.position.y;
        //Debug.Log(distanceX+"  "+distanceY);
        
        if (distanceX > 15)
        {
            extra = 4;
        }
        else if (distanceX < 5)
        {
            extra = 0;
        }
        else
        {
            extra = 2;
        }
        if (distanceX < 1 && distanceY > 4)
        {
            extra = 6;
        }
        Vector2 direction = target.position - transform.position;
        if (reverse)
        {
            angle = Mathf.Atan2(direction.y - extra - 2, direction.x) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Atan2(direction.y - extra, direction.x) * Mathf.Rad2Deg;
        }
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }
}
