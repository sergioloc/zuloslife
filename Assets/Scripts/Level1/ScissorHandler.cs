using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorHandler : MonoBehaviour
{
    public GameObject scissor;
    public Transform shootPoint;

    public void Shoot(){
        Instantiate(scissor, shootPoint.position, shootPoint.rotation);
    }
}
