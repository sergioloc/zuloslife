using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrycicleLevelValues : MonoBehaviour
{
    public static int phase = 1; // 11 phases
    private float time = 0;
    private float delay = 5f;

    void FixedUpdate(){
        time = Time.timeSinceLevelLoad - delay;
        
        if (time >= 126.55f){
            phase = 11;
            Debug.Log("Phase: 11");
        }else if (time >= 114.55f){
            phase = 10;
            Debug.Log("Phase: 10");
        }else if (time >= 106.55f){
            phase = 9;
            Debug.Log("Phase: 9");
        }else if (time >= 98.05f){
            phase = 8;
            Debug.Log("Phase: 8");
        }else /*if (time >= 90.55f)*/{
            phase = 7;
            Debug.Log("Phase: 7");
        }
        /*
        else if (time >= 78.55f){
            phase = 6;
            Debug.Log("Phase: 6");
        }else if (time >= 66.55f){
            phase = 5;
            Debug.Log("Phase: 5");
        }else if (time >= 52.74f){
            phase = 4;
            Debug.Log("Phase: 4");
        }else if (time >= 33.27f){
            phase = 3;
            Debug.Log("Phase: 3");
        }else if (time >= 19.46f){
            phase = 2;
            Debug.Log("Phase: 2");
        }else {
            Debug.Log("Phase: 1");
        }
        */
    }
}
