using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrycicleLevelValues : MonoBehaviour
{
    public static int phase = 1; // 11 phases
    private float time = 0;
    private float delay = 5f;
    public static float health = 3;
    private int lastPhase = 0;


    void FixedUpdate(){
        time = Time.timeSinceLevelLoad - delay;
        CheckPhase();
    }

    private void CheckPhase(){
        if (time >= 126.55f){
            phase = 11;

        }else if (time >= 114.55f){
            phase = 10;
        }else if (time >= 106.55f){
            phase = 9;
        }else if (time >= 98.05f){
            phase = 8;
        }else if (time >= 90.55f){
            phase = 7;
        }else if (time >= 78.55f){
            phase = 6;
        }else if (time >= 66.55f){
            phase = 5;
        }else if (time >= 52.74f){
            phase = 4;
        }else if (time >= 33.27f){
            phase = 3;
        }else if (time >= 19.46f){
            phase = 2;
        }
        PrintLastPhase();
    }

    private void CheckLastPhase(){
        if (time >= 27f){
            phase = 11;
        }else if (time >= 20f){
            phase = 10;
        }else if (time >= 14f){
            phase = 9;
        }else if (time >= 7f){
            phase = 8;
        }else {
            phase = 7;
        }
        PrintLastPhase();
    }

    private void PrintLastPhase(){
        if (lastPhase != phase){
            Debug.Log("Phase: " + phase);
            lastPhase = phase;
        }
            
    }
}
