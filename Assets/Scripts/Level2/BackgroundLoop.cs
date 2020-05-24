using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private float width = 38.4f;
    public float speed = 0f;
    public int offset = 0;
    public ParticleSystem wind, chair;
    private bool isStop = false;

    void Update(){
        if (!isStop)
            UpdateSpeed(LevelTwoValues.phase);
        if (transform.position.x < -width){
            Reposition();
        }
    }

    private void UpdateSpeed(int phase){
        if (phase == 1){
            speed = 4;
        }
        else if (phase == 2){
            speed = 6;
        }
        else if (phase == 3){
            speed = 8;
        }
        else if (phase == 4){
            speed = 10;
        }
        else if (phase == 5){
            speed = 12;
        }
        else if (phase == 6){
            speed = 14;
        }
        else if (phase == 7){
            speed = 16;
        }
        else if (phase == 9){
            speed = 20;
        }
        else if (phase == 10){
            speed = 20;
        }
        else if (phase == 11){
            speed = 0;
            isStop = true;
        }
        transform.Translate(Vector2.left * (speed+offset) * Time.deltaTime);
    }

    private void Reposition(){
        Vector2 vector = new Vector2(width * 2f, 0f);
        transform.position = (Vector2) transform.position + vector;
    }
}
