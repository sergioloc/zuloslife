using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Cooldown Times")]
    public float cooldownPanda;
    public float cooldownKero;
    public float cooldownCinamon;
    public float cooldownKutter;
    public float cooldownTrisky;
    public float speed = 0.1f;
    private float currentPanda, currentKero, currentCinamon, currentKutter, currentTrisky;
    
    [Header("Icons")]
    public Image maskPanda;
    public Image maskKero;
    public Image maskCinamon;
    public Image maskKutter;
    public Image maskTrisky;

    [Header("Characters")]
    public GameObject panda;
    public GameObject kero;
    public GameObject cinamon;
    public GameObject kutter;
    public GameObject trisky;

    void Start()
    {
        cooldownPanda = cooldownPanda * 4;
        cooldownKero = cooldownKero * 4;
        cooldownCinamon = cooldownCinamon * 4;
        cooldownKutter = cooldownKutter * 4;
        cooldownTrisky = cooldownTrisky * 4;

        currentPanda = cooldownPanda;
        currentKero = cooldownKero;
        currentCinamon = cooldownCinamon;
        currentKutter = cooldownKutter;
        currentTrisky = cooldownTrisky;
    }

    void FixedUpdate()
    {
        currentPanda = currentPanda + speed;
        maskPanda.fillAmount = currentPanda / cooldownPanda;

        currentKero = currentKero + speed;
        maskKero.fillAmount = currentKero / cooldownKero;

        currentCinamon = currentCinamon + speed;
        maskCinamon.fillAmount = currentCinamon / cooldownCinamon;

        currentKutter = currentKutter + speed;
        maskKutter.fillAmount = currentKutter / cooldownKutter;

        currentTrisky = currentTrisky + speed;
        maskTrisky.fillAmount = currentTrisky / cooldownTrisky;
    }

    public void ResetCurrent()
    {
        if (panda.activeInHierarchy && maskPanda.fillAmount == 1)
        {
            StartCoroutine(RestartPanda());
        }
        else if (kero.activeInHierarchy && maskKero.fillAmount == 1)
        {
            StartCoroutine(RestartKero());
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

    IEnumerator RestartKero()
    {
        yield return new WaitForSeconds(0.2f);
        currentKero = 0;
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
