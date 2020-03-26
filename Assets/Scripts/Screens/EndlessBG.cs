using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBG : MonoBehaviour
{
    private float speed;
    public float plus = 0f;
    public float startPos;
    public float endPos;

    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= endPos)
        {
            Vector2 p = new Vector2(startPos, transform.position.y);
            transform.position = p;
        }

        if (TrycicleLevelValues.phase == 1){
            speed = 2f + plus;
        }
        else if (TrycicleLevelValues.phase == 2){
            speed = 4f + plus;
        }
        else if (TrycicleLevelValues.phase == 3){
            speed = 6f + plus;
        }
        else if (TrycicleLevelValues.phase == 4){
            speed = 8f + plus;
        }
        else if (TrycicleLevelValues.phase == 5){
            speed = 10f + plus;
        }
        else if (TrycicleLevelValues.phase == 6){
            speed = 12f + plus;
        }
        else if (TrycicleLevelValues.phase == 7){
            speed = 14f + plus;
        }
        else if (TrycicleLevelValues.phase == 8){
            speed = 16f + plus;
        }
        else if (TrycicleLevelValues.phase == 9){
            speed = 18f + plus;
        }


    }
}
