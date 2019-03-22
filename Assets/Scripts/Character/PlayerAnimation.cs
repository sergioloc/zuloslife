using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject panda, kero;
    private Animator pandaAnim, keroAnim;
    public Joystick joystick;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //pandaAnim = panda.GetComponent<Animator>();
        keroAnim = kero.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.Horizontal >= 0.2f || joystick.Horizontal <= -0.2f)
        {
            keroAnim.SetBool("Run", true);
        }
        else{
            keroAnim.SetBool("Run", false);
        }        
    }
}
