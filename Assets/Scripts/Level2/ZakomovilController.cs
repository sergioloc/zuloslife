using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZakomovilController : MonoBehaviour
{
    public float range;
    private float maxHeight;
    private float minHeight;
    public Animator animCamera;
    public bool shake = true;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        maxHeight = range;
        minHeight = -range;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TrycicleLevelValues.phase < 10){
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDown();
            }
        }
        else {
            transform.position = new Vector2(transform.position.x, 0);
        }
    }

    public void MoveUp()
    {
        if (transform.position.y < maxHeight)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + range);
            if (shake) animCamera.SetTrigger("Shake2");
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }

    public void MoveDown()
    {
        if (transform.position.y > minHeight)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - range);
            if (shake) animCamera   .SetTrigger("Shake2");
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
