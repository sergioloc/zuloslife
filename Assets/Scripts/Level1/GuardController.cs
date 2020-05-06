using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public int health = 100;
    public float speed;
    public GameObject target, deathEffect, bloodMask;
    public ParticleSystem bloodParticle;
    private Animator guardAnimation;
    private bool lookRight = true;
    private bool freeze = false, deadEffect = false;
    private float limit = 2.3f, distance, velocity;
    private Rigidbody2D rb2d;
    Vector3 lastPosition = Vector3.zero;

    [Header("Ragdoll")]
    public GameObject explosion;
    public GameObject[] bodyParts;
    private Dissolve dissolveMat;



    void Start()
    {
        guardAnimation = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        dissolveMat = GetComponent<Dissolve>();

        if (transform.localScale.x < 0)
        {
            limit = -limit;
        }
    }

    void FixedUpdate()
    {
        if (target != null){
            distance = target.transform.position.x - transform.position.x;

        //Look
        if (distance > 0 && !lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
            limit = -limit;
        }
        else if (distance < 0 && lookRight && !freeze)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
            limit = -limit;
        }

        //Follow
        if (Mathf.Abs(distance) < 9 && !freeze)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), speed * Time.deltaTime);
            velocity = (transform.position - lastPosition).magnitude;
            lastPosition = transform.position;
        }

        //Run
        if (velocity != 0)
            guardAnimation.SetBool("Run", true);
        else
            guardAnimation.SetBool("Run", false);

        //Action
        if (Mathf.Abs(distance) < 2.45 && Mathf.Abs(distance) > 0 && !freeze)
            guardAnimation.SetBool("Action", true);
        else
            guardAnimation.SetBool("Action", false);
        }  

        if (Input.GetKeyDown(KeyCode.L))
        {
            Instantiate(bloodMask, new Vector3(transform.position.x, transform.position.y, -0.9f), Quaternion.identity);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Melee")
        {  
            bloodParticle.Play();
            TakeDamage(100);
            Push();
        }
        else if (col.gameObject.tag == "Flash")
        {
            freeze = true;
            guardAnimation.SetBool("Freeze",true);
            StartCoroutine(FinishFreeze(5f));
        }
        else if (col.gameObject.tag == "Shield")
        {
            freeze = true;
        }
        else if (col.gameObject.tag == "Explosion")
        {
            bloodParticle.Play();
            TakeDamage(100);
            Push();
            //guardAnimation.SetBool("Explosion", true);
            //StartCoroutine(FinishExplosion());
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
        else if (col.gameObject.tag == "Shield")
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

    private void Push()
    {
        if (lookRight)
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-1500, 500));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector3(1500, 500));
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

        if(health <= 0 && !deadEffect)
        {
            deadEffect = true;
            Die();
        }
    }

    private void Die()
    {
        EnableRagdoll();
        dissolveMat.Disappear();
        StartCoroutine(DestroyGameObject());
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Instantiate(bloodMask, new Vector3(transform.position.x, transform.position.y, -0.9f), Quaternion.identity);
    }

    private void EnableRagdoll(){
        guardAnimation.enabled = false;
        explosion.SetActive(true);
        gameObject.GetComponent<Collider2D>().enabled = false;
        for (int i=0; i < bodyParts.Length; i++){
            bodyParts[i].GetComponent<Rigidbody2D>().isKinematic = false;
            bodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    private IEnumerator DestroyGameObject(){
        yield return new WaitForSeconds(2);
        for (int i=0; i < bodyParts.Length; i++){
            bodyParts[i].GetComponent<Collider2D>().enabled = false;
        }
        Destroy(gameObject);
    }

}

