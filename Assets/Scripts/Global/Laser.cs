using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public string mask;
    public Transform particle;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        RaycastHit2D mHit = Physics2D.Raycast(transform.position, transform.up, 60f, 1 << LayerMask.NameToLayer(mask));
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, mHit.point);
        if (particle != null)
            particle.position = mHit.point;
    }
}
