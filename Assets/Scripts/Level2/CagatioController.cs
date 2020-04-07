using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagatioController : MonoBehaviour
{
    private Animator animCagatio;
    private int phase;

    void Start()
    {
        animCagatio = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        phase = TrycicleLevelValues.phase;

        if (phase == 9){
            animCagatio.SetBool("Claw", true);
        }
        else if (phase == 10){
            animCagatio.SetBool("Claw", false);
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            animCagatio.SetBool("Fire", true);
        }
    }
}
