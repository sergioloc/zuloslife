using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform target;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.right);
    }
}
