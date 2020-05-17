using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShakeScreen(){
        animator.SetTrigger("Shake");
    }

    public void ShakeScreenLoop(){
        animator.SetBool("isShaking", true);
    }

    public void StopShakeScreenLoop(){
        animator.SetBool("isShaking", false);
    }

    public void CenterCamera(){
        animator.SetTrigger("Center");
    }

    public void SwitchSide(){
        if (animator.GetBool("isRight"))
            animator.SetBool("isRight", false);
        else
            animator.SetBool("isRight", true);
    }
}
