using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoValues : MonoBehaviour
{
    public bool finalPhase = false;
    public static int phase = 1; // 11 phases
    public static float health = 3;
    private float time = 0;
    private int lastPhase = 0;
    private float delay = 5f;

    void Update(){
        time = Time.timeSinceLevelLoad - delay;
        if (finalPhase) CheckLastPhase();
        else CheckPhase();
    }

    private void CheckPhase(){
        if (time >= 126.55f){
            phase = 11; // phase 10 = 12 seg
        }else if (time >= 114.55f){
            phase = 10; // phase 9 = 8 seg
        }else if (time >= 106.55f){
            phase = 9; // phase 8 = 8.5 seg
        }else if (time >= 98.05f){
            phase = 8; // phase 7 = 7.5 seg
        }else if (time >= 90.55f){
            phase = 7; // phase 6 = 12 seg
        }else if (time >= 78.55f){
            phase = 6; // phase 5 = 12 seg
        }else if (time >= 66.55f){
            phase = 5; // phase 4 = 13.81 seg
        }else if (time >= 52.74f){
            phase = 4; // phase 3 = 19.47 seg
        }else if (time >= 33.27f){
            phase = 3; // phase 2 = 13.81 seg
        }else if (time >= 19.46f){
            phase = 2; // phase 1 = 19.46 seg
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
