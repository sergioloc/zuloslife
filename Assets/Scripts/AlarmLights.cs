using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLights : MonoBehaviour
{
    public GameObject colliders, door2, level1, light0, light1, light2, light3, light4, light5, light6, water;
    private bool check = false, activated = false;
    private Animator waterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        waterAnimator = water.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            check = false;
            StartCoroutine(BlinkLights());
        }
    }


    IEnumerator BlinkLights()
    {
        yield return new WaitForSeconds(1f);
        if (!light1.activeInHierarchy)
        {
            light0.SetActive(true);
            light1.SetActive(true);
            light2.SetActive(true);
            light3.SetActive(true);
            light4.SetActive(true);
            light5.SetActive(true);
            light6.SetActive(true);
        }
        else
        {
            light0.SetActive(false);
            light1.SetActive(false);
            light2.SetActive(false);
            light3.SetActive(false);
            light4.SetActive(false);
            light5.SetActive(false);
            light6.SetActive(false);
        }
        check = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            waterAnimator.SetTrigger("Start");
            colliders.SetActive(true);
            level1.SetActive(false);
            door2.SetActive(true);
            check = true;
            activated = true;
        }
    }
}
