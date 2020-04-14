using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class MissileLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.right);
    }
}
