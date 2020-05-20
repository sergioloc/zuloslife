using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private Rigidbody2D rg2d;
    private float width = 38.4f;
    public float speed = 0f;
    public int offset = 0;
    private bool isStop = false;

    void Start(){
        speed = -speed;
        rg2d = GetComponent<Rigidbody2D>();
        rg2d.velocity = new Vector2(speed, 0f);
    }

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
        else if (phase == 8){
            speed = 18;
        }
        else if (phase == 9){
            speed = 20;
        }
        else if (phase == 10){
            speed = 20;
        }
        else if (phase == 11){
            speed = -5;
            isStop = true;
        }
        speed = speed + offset;    
        SetSpeed(-speed);
    }

    private void SetSpeed(float s){
        Debug.Log(speed);
        speed = s;
        rg2d.velocity = new Vector2(speed, 0f);
    }

    IEnumerator StopSpeed()
    {
        yield return new WaitForSeconds(6f);
        SetSpeed(-speed);
    }

    private void Reposition(){
        Vector2 vector = new Vector2(width * 2f, 0f);
        transform.position = (Vector2) transform.position + vector;
    }
}
