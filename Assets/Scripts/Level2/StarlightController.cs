using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarlightController : MonoBehaviour
{
    private bool phase1, phase2, phase3, phase4, phase5, phase6, phase7, phase8, phase9;
    private int currentPhase;
    private ParticleSystem starlight;
    void Start()
    {
        starlight = GetComponent<ParticleSystem>();
        phase1 = phase2 = phase3 = phase4 = phase5 = phase6 = phase7 = phase8 = phase9 = false;
    }

    void FixedUpdate()
    {
        currentPhase = LevelTwoValues.phase;
        if (currentPhase == 1 && !phase1){
            starlight.Play();
            phase1 = true;
        }
        else if (currentPhase == 2 && !phase2){
            starlight.Play();
            phase2 = true;
        }
        else if (currentPhase == 3 && !phase3){
            starlight.Play();
            phase3 = true;
        }
        else if (currentPhase == 4 && !phase4){
            starlight.Play();
            phase4 = true;
        }
        else if (currentPhase == 5 && !phase5){
            starlight.Play();
            phase5 = true;
        }
        else if (currentPhase == 6 && !phase6){
            starlight.Play();
            phase6 = true;
        }
        else if (currentPhase == 7 && !phase7){
            starlight.Play();
            phase7 = true;
        }
        else if (currentPhase == 8 && !phase8){
            starlight.Play();
            phase8 = true;
        }
        else if (currentPhase == 9 && !phase9){
            starlight.Play();
            phase9= true;
        }
    }
}
