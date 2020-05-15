using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public string mask;
    public string mask2;
    public Transform particle;
    private int layermask1, layermask2, layermask;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        layermask1 = 1 << LayerMask.NameToLayer(mask);
        layermask2 = 1 << LayerMask.NameToLayer(mask2);
        layermask = layermask1 | layermask2;
    }

    void Update()
    {
        RaycastHit2D mHit = Physics2D.Raycast(transform.position, transform.up, 60f, layermask);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, mHit.point);
        if (particle != null)
            particle.position = mHit.point;
    }
}
