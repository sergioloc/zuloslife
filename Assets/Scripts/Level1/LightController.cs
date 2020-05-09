using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Experimental.Rendering.Universal.Light2D light2D;

    public void ChangeBossColor(){
        Color32 red = new Color32(255, 45, 45, 255);
        if (light2D.color != red)
        {
            light2D.color = red;
            light2D.intensity = 1.5f;
        }
        else
        {
            light2D.color = new Color32(255, 137, 42, 255);
            light2D.intensity = 1f;
        }
        
    }
}
