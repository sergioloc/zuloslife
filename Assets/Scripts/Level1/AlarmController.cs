using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchLight(){
        if (animator.GetBool("On")){
            animator.SetBool("On", false);
        }
        else{
            animator.SetBool("On", true);
        }
    }
}
