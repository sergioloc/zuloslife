using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public int health = 100;
    public float speed;
    public GameObject player, deathEffect, bloodEffect1, bloodEffect2;
    public ParticleSystem bloodParticle;
    private Animator guardAnimation;
    private bool lookRight = true;
    private bool freeze = false;
    private float limit = 2.3f, distance, realDistance;

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
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            realDistance = player.transform.position.x - transform.position.x;

            //Look
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

            //Move
            if (distance < 9 && !freeze)
            {
                guardAnimation.SetBool("Run", true);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
            if (distance < 2.45 && distance > 0 && gameObject.GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                guardAnimation.SetBool("Run", true);
                guardAnimation.SetBool("Action", true);
            }
            else if (distance < 2.45 && distance > 0)
            {
                guardAnimation.SetBool("Run", false);
                guardAnimation.SetBool("Action", true);
            }
            else
            {
                guardAnimation.SetBool("Action", false);
            }

            if (distance > 9)
            {
                guardAnimation.SetBool("Run", false);

            }
        }
        else
        {
            guardAnimation.SetBool("Run", false);
            guardAnimation.SetBool("Action", false);
        }
        
    }

    private void Push()
    {
        if (lookRight)
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-1500, 500));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector3(1500, 500));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Flash")
        {
            freeze = true;
            guardAnimation.SetBool("Freeze",true);
            StartCoroutine(FinishFreeze(5f));
        }
        else if (col.gameObject.tag == "Melee")
        {  
            bloodParticle.Play();
            TakeDamage(20);
            Push();
        }
        else if (col.gameObject.tag == "Explosion")
        {
            guardAnimation.SetBool("Explosion", true);
            StartCoroutine(FinishExplosion());
        }
        else if (col.gameObject.tag == "Guard")
        {
            freeze = true;
            guardAnimation.SetBool("Run", false);
        }
        else if (col.gameObject.tag == "PlayerDeath")
        {
            freeze = true;
            StartCoroutine(FinishFreeze(7f));
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Guard")
        {
            freeze = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            bloodParticle.Play();
            TakeDamage(10);
            Push();
        }
    }


private IEnumerator FinishFreeze(float sec)
    {
        yield return new WaitForSeconds(sec);
        freeze = false;
        guardAnimation.SetBool("Freeze", false);
    }

    private IEnumerator FinishExplosion()
    {
        yield return new WaitForSeconds(1);
        guardAnimation.SetBool("Explosion", false);
    }

    private void TakeDamage(int damage)
    {
        health = health - damage;

        if(health <= 0)
        {
            Die();
        }
    }
    //-0.9
    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        

        if (Random.Range(1, 2) == 1)
        {
            Instantiate(bloodEffect1, new Vector3(transform.position.x, transform.position.y, -0.9f), Quaternion.identity);
        }
        else
        {
            Instantiate(bloodEffect2, new Vector3(transform.position.x, transform.position.y, -0.9f), Quaternion.identity);
        }

    }

}

