using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBreath : MonoBehaviour
{
    public GameObject waterBar;
    private Slider waterSlider;
    public int Oxygen = 100;
    private bool checkOxy = false, checkHealth = true;

    void Start()
    {
        waterSlider = waterBar.GetComponent<Slider>();
    }

    void Update()
    {
        waterSlider.value = Oxygen;
        if (Oxygen <= 0 && checkHealth)
        {
            checkHealth = false;
            StartCoroutine(DecrementHealth());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water") && !checkOxy)
        {
            StartCoroutine(DecrementOxygen());
        }
        if (collision.gameObject.CompareTag("Air"))
        {
            StartCoroutine(RestoreBreath());
        }
    }

    IEnumerator RestoreBreath()
    {
        Oxygen = 100;
        yield return new WaitForSeconds(2f);
    }

    IEnumerator DecrementOxygen()
    {
        waterBar.SetActive(true);
        checkOxy = true;
        yield return new WaitForSeconds(0.001f);
        Oxygen--;
        checkOxy = false;
    }

    IEnumerator DecrementHealth()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.instance.health--;
        checkHealth = true;
    }
}
