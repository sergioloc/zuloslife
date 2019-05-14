using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public GameObject player;
    private float limit = 3f;
    public float speed;
    private bool lookRight = true;
    private bool freeze = false;
    private Animator guardAnimation;

    //private bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        guardAnimation = GetComponent<Animator>();

        if (transform.localScale.x < 0)
        {
            limit = -limit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float x1 = player.transform.position.x;
        float x2 = transform.position.x;
        float realDistance = x1 - x2;

        //Position
        if (realDistance > 0 && !lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
            limit = -limit;
        }
        else if (realDistance < 0 && lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
            limit = -limit;
        }
        if (distance < 5 && !freeze)
        {
            guardAnimation.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x - limit, transform.position.y), speed * Time.deltaTime);
        }
        if(distance < 3.2 && distance > 3.1)
        {
            guardAnimation.SetBool("Run", false);
            guardAnimation.SetBool("Action", true);
        }
        else
        {
            guardAnimation.SetBool("Action", false);
        }

        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Flash")
        {
            freeze = true;
            guardAnimation.SetBool("Freeze",true);
            StartCoroutine(Wait());
        }
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        freeze = false;
        guardAnimation.SetBool("Freeze", false);
    }

}
