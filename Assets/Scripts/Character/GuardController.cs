using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public GameObject player, deathEffect, bloodEffect1, bloodEffect2;
    private float limit = 2.3f;
    public float speed;
    private bool lookRight = true;
    private bool freeze = false;
    private Animator guardAnimation;
    public ParticleSystem bloodParticle;
    public int health = 100;



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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
        //Debug.Log(distance);
        if(distance < 2.45 && distance > 2.35)
        {
            guardAnimation.SetBool("Run", false);
            guardAnimation.SetBool("Action", true);
        }
        else
        {
            guardAnimation.SetBool("Action", false);
        }
        if(distance > 5)
        {
            guardAnimation.SetBool("Run", false);

        }


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Flash")
        {
            freeze = true;
            guardAnimation.SetBool("Freeze",true);
            StartCoroutine(finishFreeze());
        }
        if (col.gameObject.tag == "Weapon")
        {  
            bloodParticle.Play();
            TakeDamage(20);
            push();
        }
        if (col.gameObject.CompareTag("Explosion"))
        {
            guardAnimation.SetBool("Explosion", true);
            StartCoroutine(finishExplosion());
        }
    }

    private void push()
    {
        if (lookRight)
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-1500, 500));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector3(1500, 500));
    }


    IEnumerator finishFreeze()
    {
        yield return new WaitForSeconds(5);
        freeze = false;
        guardAnimation.SetBool("Freeze", false);
    }

    IEnumerator finishExplosion()
    {
        yield return new WaitForSeconds(1);
        guardAnimation.SetBool("Explosion", false);

    }

    public void TakeDamage(int damage)
    {
        health = health - damage;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (Random.Range(1, 2) == 1)
        {
            Instantiate(bloodEffect1, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(bloodEffect2, transform.position, Quaternion.identity);
        }

    }

}

