using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaController : MonoBehaviour
{
    private Image bar;

    void Start()
    {
        bar = gameObject.GetComponent<Image>();
        bar.fillAmount = 0;
    }

    void Update()
    {  
        if (bar.fillAmount < 1)
        {
            //bar.fillAmount = Time.realtimeSinceStartup / 50;
        }
        
    }
}
