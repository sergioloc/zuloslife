using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrycicleLevelValues : MonoBehaviour
{
    public static int phase = 1; // 9 phases
    private float time = 0;

    void FixedUpdate(){
        time = Time.timeSinceLevelLoad;
        
        if (time >= 114.2f){
            phase = 9;
            Debug.Log("Phase: 9");
        }else if (time >= 92f){
            phase = 8;
            Debug.Log("Phase: 8");
        }else if (time >= 69.15f){
            phase = 7;
            Debug.Log("Phase: 7");
        }else if (time >= 57.22f){
            phase = 6;
            Debug.Log("Phase: 6");
        }else if (time >= 44.17f){
            phase = 5;
            Debug.Log("Phase: 5");
        }else if (time >= 36f){
            phase = 4;
            Debug.Log("Phase: 4");
        }else if (time >= 23.13f){
            phase = 3;
            Debug.Log("Phase: 3");
        }else if (time >= 13.09f){
            phase = 2;
            Debug.Log("Phase: 2");
        }else {
            Debug.Log("Phase: 1");
        }
    }
}
