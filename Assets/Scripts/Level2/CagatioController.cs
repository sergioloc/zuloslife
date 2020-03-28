using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagatioController : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("Shield");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Fire");
        }
    }
}
