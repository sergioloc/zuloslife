using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeSide : MonoBehaviour
{
    public GameObject side1, side2, spawner;
    public UnityEvent OnSwitch, OnCenter;

    public void UpdateCameraPosition(){
        if (LevelTwoValues.phase == 2){ //right
            side2.SetActive(true);
            MoveRight();
        }
        else if (LevelTwoValues.phase == 3){ // left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 4){ // right
            side2.SetActive(true);
            MoveRight();
        }
        else if (LevelTwoValues.phase == 5){ //left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 6){ //right
            side2.SetActive(true);
            MoveRight();
        }
        else if (LevelTwoValues.phase == 7){ //left
            if (!side1.activeSelf){
                side1.SetActive(true);
                MoveLeft();
            }
            spawner.SetActive(false);
        }
        else if (LevelTwoValues.phase == 8){ //left
            spawner.SetActive(true);
            Spawner.wait = false;
        }
        else if (LevelTwoValues.phase == 9){ //right
            side2.SetActive(true);
            MoveRight();
        }
        else if (LevelTwoValues.phase == 10){ //center
            side1.SetActive(true);
            OnCenter.Invoke();
        }
    }

    private void MoveRight(){
        spawner.SetActive(false);
        OnSwitch.Invoke();
        StartCoroutine(HideSide(side1));
    }

    private void MoveLeft(){
        spawner.SetActive(true);
        OnSwitch.Invoke();
        StartCoroutine(HideSide(side2));
        Spawner.wait = false;
    }

    IEnumerator HideSide(GameObject side)
    {
        yield return new WaitForSeconds(1f);
        side.SetActive(false);
    }
}
