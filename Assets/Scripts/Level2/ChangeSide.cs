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
        if (TrycicleLevelValues.phase == 2 && !side2.activeSelf){ //right
            side2.SetActive(true);
            MoveRight();
            DirectorCombat.wait = true;
        }
        else if (TrycicleLevelValues.phase == 3 && !side1.activeSelf){ // left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (TrycicleLevelValues.phase == 4 && !side2.activeSelf){ // right
            side2.SetActive(true);
            MoveRight();
            DirectorCombat.wait = true;
        }
        else if (TrycicleLevelValues.phase == 5 && !side1.activeSelf){ //left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (TrycicleLevelValues.phase == 6 && !side2.activeSelf){ //right
            side2.SetActive(true);
            MoveRight();
            DirectorCombat.wait = true;
        }
        else if (TrycicleLevelValues.phase == 7 && !side1.activeSelf){ //left
            side1.SetActive(true);
            MoveLeft();
        }
        else if (TrycicleLevelValues.phase == 9 && !side2.activeSelf){ //right
            side2.SetActive(true);
            MoveRight();
            DirectorCombat.wait = true;
        }
        else if (TrycicleLevelValues.phase == 10){ //center
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
        DirectorCombat.wait = false;
    }


}
