using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakomovilController : MonoBehaviour
{
    void OnEnable()
    {
        if (LevelTwoValues.phase >= 10){
            transform.position = new Vector2(transform.position.x, 0);
        }
    }
}
