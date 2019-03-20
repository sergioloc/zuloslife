using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public GameObject panda;
    public GameObject kero;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {  
    }

    public void switchToPanda()
    {
        panda.SetActive(true);
        kero.SetActive(false);
    }

    public void switchToKero()
    {
        panda.SetActive(false);
        kero.SetActive(true);
    }
}
