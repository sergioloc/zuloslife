using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBreath : MonoBehaviour
{
    private Slider waterSlider;
    public GameObject waterBar;
    public int Oxygen = 100;
    private bool checkOxy = true, checkHealth = true;

    // Start is called before the first frame update
    void Start()
    {
        waterSlider = waterBar.GetComponent<Slider>();
    }

    // Update is called once per frame
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
        if (collision.gameObject.CompareTag("Water"))
        {
            waterBar.SetActive(true);
            if (checkOxy)
            {
                checkOxy = false;
                StartCoroutine(DecrementOxygen());
            }
        }
    }

    IEnumerator DecrementOxygen()
    {
        yield return new WaitForSeconds(0.1f);
        Oxygen--;
        checkOxy = true;
    }

    IEnumerator DecrementHealth()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.instance.health--;
        checkHealth = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Oxygen =  100;
        }
    }

}
