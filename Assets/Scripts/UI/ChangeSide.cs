using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSide : MonoBehaviour
{
    private Animator animCam;
    public GameObject side1, side2;

    void Start()
    {
        animCam = GetComponent<Animator>();
    }

    void Update()
    {
        CheckPhase();
    }

    private void CheckPhase(){
        if (TrycicleLevelValues.phase == 2 && !side2.activeSelf){
            side2.SetActive(true);
            StartCoroutine(MoveRight());
        }
        else if (TrycicleLevelValues.phase == 3 && !side1.activeSelf){
            side1.SetActive(true);
            StartCoroutine(MoveLeft());
        }
        else if (TrycicleLevelValues.phase == 4 && !side2.activeSelf){
            side2.SetActive(true);
            StartCoroutine(MoveRight());
        }
        else if (TrycicleLevelValues.phase == 5 && !side1.activeSelf){
            side1.SetActive(true);
            StartCoroutine(MoveLeft());
        }
        else if (TrycicleLevelValues.phase == 6 && !side2.activeSelf){
            side2.SetActive(true);
            StartCoroutine(MoveRight());
        }
        else if (TrycicleLevelValues.phase == 7 && !side1.activeSelf){
            side1.SetActive(true);
            StartCoroutine(MoveLeft());
        }
        else if (TrycicleLevelValues.phase == 8 && !side2.activeSelf){
            side2.SetActive(true);
            StartCoroutine(MoveRight());
        }
        else if (TrycicleLevelValues.phase == 9 && !side1.activeSelf){
            side1.SetActive(true);
            StartCoroutine(MoveLeft());
        }
    }

    IEnumerator MoveRight(){
        yield return new WaitForSeconds(1f);
        animCam.SetBool("isRight", true);
        StartCoroutine(HideSide(side1));
    }

    IEnumerator MoveLeft(){
        yield return new WaitForSeconds(1f);
        animCam.SetBool("isRight", false);
        StartCoroutine(HideSide(side2));
    }


    IEnumerator HideSide(GameObject side)
    {
        yield return new WaitForSeconds(1f);
        side.SetActive(false);
    }


}
