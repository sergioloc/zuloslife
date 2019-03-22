using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtons : MonoBehaviour
{
    public GameObject kero;
    private Animator keroAnimation;

    // Start is called before the first frame update
    void Start()
    {
        keroAnimation = kero.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAction()
    {
        keroAnimation.SetBool("Action", true);
    }

    public void stopAction()
    {
        keroAnimation.SetBool("Action", false);

    }
}
