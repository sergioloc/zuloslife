using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class GuardController : MonoBehaviour
{
    public int health = 100;
    public float speed = 9;
    public float timeFreeze = 5f;
    public GameObject deathEffect, bloodMask, eyesFreeze;
    private GameObject target;
    public ParticleSystem bloodParticle;
    private Animator guardAnimation;
    private bool lookRight = true;
    private bool freeze = false, deadEffect = false;
    private float limit = 2.3f, distance, velocity;
    private Rigidbody2D rb2d;
    Vector3 lastPosition = Vector3.zero;

    [Header("Sounds")]
    public AudioSource walkSound;
    public AudioSource attackSound;
    public AudioSource dieSound;

    [Header("Ragdoll")]
    public GameObject explosion;
    public GameObject[] bodyParts;
    private Dissolve dissolveMat;

    void Start()
    {
        target = GameObject.Find("Player");
        guardAnimation = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        dissolveMat = GetComponent<Dissolve>();

        if (transform.localScale.x < 0)
        {
            limit = -limit;
        }
        speed = ConfigManager.appConfig.GetInt("guardSpeed");
        if (speed == 0) speed = 9;
    }

    void Update()
    {
        if (target != null && LevelOneValues.isPlayerAlive){
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
            if (Mathf.Abs(distance) < 9)
            {
                if (!freeze)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.transform.position.x - limit, transform.position.y), speed * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.transform.position.x - limit, transform.position.y), 0.001f * Time.deltaTime);
                velocity = (transform.position - lastPosition).magnitude;
                lastPosition = transform.position;
            }

            //Run
            if (velocity != 0 && !freeze && health > 0){
                guardAnimation.SetBool("Run", true);
                if (!walkSound.isPlaying)
                    walkSound.Play();
            }
                
            else {
                guardAnimation.SetBool("Run", false);
                walkSound.Stop();
            }

            //Action
            if (Mathf.Abs(distance) < 2.45 && Mathf.Abs(distance) > 0 && !freeze){
                if (!attackSound.isPlaying)
                    attackSound.Play();
                guardAnimation.SetBool("Action", true);
            }   
            else
                guardAnimation.SetBool("Action", false);
                attackSound.Stop();
            }
        else{
            guardAnimation.SetBool("Action", false);
            guardAnimation.SetBool("Run", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "WeaponSoft")
        {
            bloodParticle.Play();
            TakeDamage(50);
            Push();
        }
        else if (col.gameObject.tag == "WeaponMedium")
        {
            Debug.Log("WeaponMedium");
            Debug.Log(col.gameObject.name);
            bloodParticle.Play();
            TakeDamage(100);
            Push();
        }
        else if (col.gameObject.tag == "WeaponHard")
        {
            bloodParticle.Play();
            TakeDamage(100);
            Push();
        }
        else if (col.gameObject.tag == "Flash")
        {
            StartCoroutine(Freeze(true));
        }
        else if (col.gameObject.tag == "Shield")
        {
            freeze = true;
        }
        else if (col.gameObject.tag == "Enemy")
        {
            freeze = true;
            guardAnimation.SetBool("Run", false);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            freeze = false;
        }
        else if (col.gameObject.tag == "Shield")
        {
            freeze = false;
        }
    }

    private void Push()
    {
        if (lookRight)
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-1500, 500));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector3(1500, 500));
    }


    IEnumerator Freeze(bool eyes)
    {
        freeze = true;
        if (eyes)
            eyesFreeze.SetActive(true);
        guardAnimation.SetBool("Freeze", true);
        yield return new WaitForSeconds(timeFreeze);

        freeze = false;
        eyesFreeze.SetActive(false);
        guardAnimation.SetBool("Freeze", false);
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
        dieSound.Play();
        eyesFreeze.SetActive(false);
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

