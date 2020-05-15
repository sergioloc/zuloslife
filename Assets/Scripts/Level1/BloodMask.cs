using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMask : MonoBehaviour
{
    public Sprite[] blood;

    void Start()
    {
        int rand = Random.Range(0, blood.Length);
        GetComponent<SpriteRenderer>().sprite = blood[rand];
    }

    public void HideBlood(){
        gameObject.SetActive(false);
    }
}
