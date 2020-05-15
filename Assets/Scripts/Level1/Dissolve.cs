using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public GameObject[] parts;
    private Material[] materials;
    bool isExecuting = false, isDisappearing = false;
    float fade = 1f;

    void Start()
    {
        materials = new Material[parts.Length];
        for (int i = 0; i < parts.Length; i++){
            materials[i] = parts[i].GetComponent<SpriteRenderer>().material;
        }
    }

   
    void FixedUpdate()
    {
        if (isExecuting){
            if (isDisappearing){
                fade -= Time.deltaTime;
                if (fade <= 0f){
                    fade = 0f;
                    isExecuting = false;
                }
            }
            else{
                fade += Time.deltaTime;
                if (fade >= 1f){
                    fade = 1f;
                    isExecuting = false;
                }
            }
            
            for (int i = 0; i < materials.Length; i++){
                materials[i].SetFloat("_Fade", fade);
            }
        }
    }

    public void Disappear(){
        isExecuting = true;
        isDisappearing = true;
    }

    public void Appear(){
        isExecuting = true;
        isDisappearing = false;
    }   
}
