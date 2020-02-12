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
        if (Input.GetKeyDown(KeyCode.M))
        {
            animCam.SetTrigger("MoveRight");
            side2.SetActive(true);
            StartCoroutine(HideSide(side1));
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            animCam.SetTrigger("MoveLeft");
            side1.SetActive(true);
            StartCoroutine(HideSide(side2));

        }
    }


    IEnumerator HideSide(GameObject side)
    {
        yield return new WaitForSeconds(1f);
        side.SetActive(false);
    }


}
