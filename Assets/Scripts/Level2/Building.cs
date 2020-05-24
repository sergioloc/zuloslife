using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int speed = 0;
    public Sprite[] sprites;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[LevelTwoValues.numBuilding];
        LevelTwoValues.numBuilding++;
        speed = GetSpeed(LevelTwoValues.phase);
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private int GetSpeed(int phase){
        if (phase == 1){
            return 2;
        }
        else if (phase == 2){
            return 4;
        }
        else if (phase == 3){
            return 6;
        }
        else if (phase == 4){
            return 8;
        }
        else if (phase == 5){
            return 10;
        }
        else if (phase == 6){
            return 12;
        }
        else if (phase == 7){
            return 14;
        }
        else if (phase == 9){
            return 16;
        }
        else if (phase == 10){
            return 18;
        }
        else {
            return 0;
        }
    }
}
