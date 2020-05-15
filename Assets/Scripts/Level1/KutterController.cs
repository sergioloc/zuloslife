using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KutterController : MonoBehaviour
{
    public Transform body;
    public float rotateSpeed;
    public GameObject marker;

    [Header("Targets in area")]
    public List<Transform> targets;

    public void Shoot(){
        int index = NearestTarget();
        if (index != -1)
            StartCoroutine(LookAt(targets[index].position));
    }

    IEnumerator LookAt(Vector2 lookAtPosition)
    {
        float distanceX = lookAtPosition.x - transform.position.x;
        float distanceY = lookAtPosition.y - transform.position.y;
        float angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;
        distanceX = lookAtPosition.x - transform.position.x;
        distanceY = lookAtPosition.y - transform.position.y;
        angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;
        Quaternion endRotation = Quaternion.AngleAxis(angle + 180, Vector3.back);
        body.rotation = Quaternion.Slerp(body.rotation, endRotation, Time.deltaTime * rotateSpeed);

        yield return new WaitForSeconds(1f);

        body.localRotation = Quaternion.Euler(0,0,0);
    }

    private int NearestTarget()
    {
        float[] distances = new float[targets.Count];

        for (int i = 0; i < targets.Count; i++)
        {
            distances[i] = (Mathf.Abs(targets[i].position.x - transform.position.x));
        }

        float minDistance = Mathf.Min(distances);
        int index = -1;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }
        return index;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lab"))
        {
            if (!targets.Contains(collision.transform))
                targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lab"))
        {
            if (targets.Contains(collision.transform)){
                targets.Remove(collision.transform);
            }
        }
    }
}
