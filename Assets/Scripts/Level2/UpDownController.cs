using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpDownController : MonoBehaviour
{
    public int range;
    public bool shake = true, freeze = false;
    public GameObject particle;
    private int minHeight, maxHeight;
    private Animator animator;
    public UnityEvent OnShake;

    void Start()
    {
        animator = GetComponent<Animator>();
        maxHeight = range;
        minHeight = -range;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            MoveDown();
        }
    }

    public void MoveUp(){
        if (transform.position.y < maxHeight && !freeze && LevelTwoValues.phase < 10)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + range);
            if (shake) OnShake.Invoke();
            if (particle != null) Instantiate(particle, transform.position, Quaternion.identity);
        }
    }

    public void MoveDown(){
        if (transform.position.y > minHeight && !freeze && LevelTwoValues.phase < 10)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - range);
            if (shake) OnShake.Invoke();
            if (particle != null) Instantiate(particle, transform.position, Quaternion.identity);
        }
    }

    public void Freeze(){
        freeze = true;
    }

    public void UnFreeze(){
        freeze = false;
    }
}
