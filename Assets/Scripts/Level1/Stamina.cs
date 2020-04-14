using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float maxPanda, maxKero, maxCinamon, maxKutter, maxTrisky;
    public float speed = 0.1f;
    private float currentPanda, currentKero, currentCinamon, currentKutter, currentTrisky;
    public Image maskPanda, maskKero, maskCinamon, maskKutter, maskTrisky;
    public GameObject panda, kero, cinamon, kutter, trisky;


    // Start is called before the first frame update
    void Start()
    {
        currentPanda = maxPanda;
        currentKero = maxKero;
        currentCinamon = maxCinamon;
        currentKutter = maxKutter;
        currentTrisky = maxTrisky;
    }

    // Update is called once per frame
    void Update()
    {
        currentPanda = currentPanda + speed;
        maskPanda.fillAmount = currentPanda / maxPanda;

        currentKero = currentKero + speed;
        maskKero.fillAmount = currentKero / maxKero;

        currentCinamon = currentCinamon + speed;
        maskCinamon.fillAmount = currentCinamon / maxCinamon;

        currentKutter = currentKutter + speed;
        maskKutter.fillAmount = currentKutter / maxKutter;

        currentTrisky = currentTrisky + speed;
        maskTrisky.fillAmount = currentTrisky / maxTrisky;
    }

    public void ResetCurrent()
    {
        if (panda.activeInHierarchy && maskPanda.fillAmount == 1)
        {
            StartCoroutine(RestartPanda());
        }
        else if (kero.activeInHierarchy)
        {
            currentKero = 0;
        }
        else if (cinamon.activeInHierarchy && maskCinamon.fillAmount == 1)
        {
            StartCoroutine(RestartCinamon());
        }
        else if (kutter.activeInHierarchy && maskKutter.fillAmount == 1)
        {
            StartCoroutine(RestartKutter());
        }
        else if (trisky.activeInHierarchy && maskTrisky.fillAmount == 1)
        {
            StartCoroutine(RestartTrisky());
        }

    }

    IEnumerator RestartPanda()
    {
        yield return new WaitForSeconds(0.8f);
        currentPanda = 0;
    }

    IEnumerator RestartCinamon()
    {
        yield return new WaitForSeconds(1.5f);
        currentCinamon = 0;
    }

    IEnumerator RestartKutter()
    {
        yield return new WaitForSeconds(0.6f);
        currentKutter = 0;
    }

    IEnumerator RestartTrisky()
    {
        yield return new WaitForSeconds(2);
        currentTrisky = 0;
    }
}
