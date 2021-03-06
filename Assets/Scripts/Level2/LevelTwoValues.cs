﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelTwoValues : MonoBehaviour
{
    public bool finalPhase = false;
    public static int phase; // 11 phases
    public static float health;
    public static float timeBtwSpawn;
    public static int numBuilding;
    private float time;
    private int lastPhase;
    private float delay = 5f;
    public UnityEvent OnNewPhase;

    void Start(){
        phase = 1;
        health = 3;
        time = 0;
        lastPhase = 0;
        timeBtwSpawn = 2f;
        numBuilding = 0;
        StartCoroutine(Tempo());
    }

    void Update(){
        time = Time.timeSinceLevelLoad - delay;
        if (finalPhase) CheckLastPhase();
        else CheckPhase();
    }

    private void CheckPhase(){
        if (time >= 126.55f){
            StartCoroutine(GoToMenu());
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

    private IEnumerator GoToMenu(){
        yield return new WaitForSeconds(15f);
        SceneManager.LoadScene("Index");
    }

    private void CheckLastPhase(){
        if (time >= 30f){
            phase = 11;
        }else if (time >= 25f){
            phase = 10;
        }else if (time >= 20f){
            phase = 9;
        }else if (time >= 15f){
            phase = 8;
        }else if (time >= 10f){
            phase = 7;
        }else if (time >= 5f){
            phase = 6;
        }else {
            phase = 5;
        }
        PrintLastPhase();
    }

    private void PrintLastPhase(){
        if (lastPhase != phase){
            OnNewPhase.Invoke();
            Debug.Log("Phase: " + phase);
            lastPhase = phase;
        }
            
    }

    IEnumerator Tempo(){
        yield return new WaitForSeconds(delay);
        float timeAdded = 1.5f / 67;
        while(true){
            if (timeBtwSpawn > 0.5)
                timeBtwSpawn = timeBtwSpawn - timeAdded;
            yield return new WaitForSeconds(1f);
        }
    }
}
