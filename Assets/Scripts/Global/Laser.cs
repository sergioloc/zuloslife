using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D mHit = Physics2D.Raycast(transform.position, transform.up, 60f, 1 << LayerMask.NameToLayer("Wall"));
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, mHit.point);
    }
}
