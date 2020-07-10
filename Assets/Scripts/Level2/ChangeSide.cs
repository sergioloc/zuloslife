using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeSide : MonoBehaviour
{
    public GameObject side1, side2, kekeo, spawner;
    public Collider2D colliderKekeo, colliderCagatio;
    public ParticleSystem starlightL, starlightR;
    public UnityEvent OnSwitch, OnCenter;

    void Start(){
        side1.SetActive(true);
        side2.SetActive(false);
    }

    public void UpdateCameraPosition(){
        if (LevelTwoValues.phase == 2){ //right
            MoveRight();
        }
        else if (LevelTwoValues.phase == 3){ // left
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 4){ // right
            MoveRight();
        }
        else if (LevelTwoValues.phase == 5){ //left
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 6){ //center
            MoveCenter();
            Spawner.wait = false;
        }
        else if (LevelTwoValues.phase == 7){
            Spawner.wait = true;
        }
        else if (LevelTwoValues.phase == 8){
            spawner.SetActive(true);
            Spawner.wait = false;
        }
        else if (LevelTwoValues.phase == 9){
            spawner.SetActive(false);
        }
    }

    private void MoveRight(){
        starlightR.Play();
        colliderKekeo.enabled = false;
        colliderCagatio.enabled = true;
        side2.SetActive(true);
        spawner.SetActive(false);
        OnSwitch.Invoke();
        StartCoroutine(HideSide(side1));
    }

    private void MoveLeft(){
        starlightL.Play();
        colliderKekeo.enabled = true;
        colliderCagatio.enabled = false;
        side1.SetActive(true);
        spawner.SetActive(true);
        OnSwitch.Invoke();
        StartCoroutine(HideSide(side2));
        Spawner.wait = false;
    }

    private void MoveCenter(){
        side2.SetActive(true);
        spawner.SetActive(true);
        OnSwitch.Invoke();
        OnCenter.Invoke();
        StartCoroutine(MoveKekeo());
    }

    IEnumerator MoveKekeo()
    {
        yield return new WaitForSeconds(3f);
        kekeo.transform.position = new Vector2(-8f, 0f);
    }

    IEnumerator HideSide(GameObject side)
    {
        yield return new WaitForSeconds(1f);
        side.SetActive(false);
    }
}
