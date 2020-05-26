using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private float width = 38.4f;
    public float speed = 0f;
    public int offset = 0;

    void Update(){
        if (transform.position.x < -width){
            Reposition();
        }
    }

    void FixedUpdate(){
        speed = GetSpeed(LevelTwoValues.phase) + offset;
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

    private void Reposition(){
        Vector2 vector = new Vector2(width * 2f, 0f);
        transform.position = (Vector2) transform.position + vector;
    }
}
