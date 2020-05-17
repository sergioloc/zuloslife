using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeSide : MonoBehaviour
{
    public GameObject side1, side2, spawner;
    public UnityEvent OnSwitch, OnCenter;

    void Update()
    {
        CheckPhase();
    }

    private void CheckPhase(){
        if (LevelTwoValues.phase == 2 && !side2.activeSelf){ //right
            side2.SetActive(true);
            MoveRight();
            EngineController.wait = true;
        }
        else if (LevelTwoValues.phase == 3 && !side1.activeSelf){ // left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 4 && !side2.activeSelf){ // right
            side2.SetActive(true);
            MoveRight();
            EngineController.wait = true;
        }
        else if (LevelTwoValues.phase == 5 && !side1.activeSelf){ //left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 6 && !side2.activeSelf){ //right
            side2.SetActive(true);
            MoveRight();
            EngineController.wait = true;
        }
        else if (LevelTwoValues.phase == 7 && !side1.activeSelf){ //left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (LevelTwoValues.phase == 9 && !side2.activeSelf){ //right
            side2.SetActive(true);
            MoveRight();
            EngineController.wait = true;
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
        StartCoroutine(AllowAttack());
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

    IEnumerator AllowAttack()
    {
        yield return new WaitForSeconds(1f);
        EngineController.wait = false;
    }


}
