using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBG : MonoBehaviour
{
    public float speed;
    public float startPos;
    public float endPos;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= endPos)
        {
            Vector2 p = new Vector2(startPos, transform.position.y);
            transform.position = p;
        }
    }
}
