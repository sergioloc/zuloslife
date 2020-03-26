using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform hit;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D mHit = Physics2D.Raycast(transform.position, transform.up);
        hit.position = mHit.point;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.position);
    }
}
