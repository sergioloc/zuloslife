using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSide : MonoBehaviour
{
    private Animator animCam;
    public GameObject side1, side2, spawner;

    void Start()
    {
        animCam = GetComponent<Animator>();
    }

    void Update()
    {
        CheckPhase();
    }

    private void CheckPhase(){
        Debug.Log(LevelTwoValues.phase);
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
            MoveCenter();
        }
    }

    private void MoveRight(){
        spawner.SetActive(false);
        animCam.SetBool("isRight", true);
        StartCoroutine(HideSide(side1));
        StartCoroutine(AllowAttack());
    }

    private void MoveLeft(){
        spawner.SetActive(true);
        animCam.SetBool("isRight", false);
        StartCoroutine(HideSide(side2));
        Spawner.wait = false;
    }

    private void MoveCenter(){
        animCam.SetTrigger("Center");
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
