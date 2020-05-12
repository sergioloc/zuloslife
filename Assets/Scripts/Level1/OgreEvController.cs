using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Events;

public class OgreEvController : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;
    public int initialHealth = 1;
    public GameObject healthBar;
    private Slider healthSlider;
    public int damage = 50;

    [Header("Movement")]
    public float speed = 50;
    public float limit = 5f;

    [Header("Attack")]
    public GameObject lasers;
    public GameObject target;
    public float laserDuration = 6f;
    public int timeCooldown = 5;

    [Header("Particles")]
    public ParticleSystem lava;
    public GameObject implosion;
    public GameObject death;
    
    
    public GameObject pre, jumpPad, door, floor6;
    private Animator ogreEvAnimation;
    private float distance;
    private bool moveLeft, moveRight, followPlayer, lookRight, cooldown, freeze;
    private Rigidbody2D rb2d;   
    private Vector3 initialPosition;
    public UnityEvent OnEvolutionSpawn;

    void Start()
    {
        OpenSecondScreen();
        initialPosition = new Vector3(0f,0f,0f);
        rb2d = GetComponent<Rigidbody2D>();
        healthSlider = healthBar.GetComponent<Slider>();
        ogreEvAnimation = GetComponent<Animator>();
        OnEvolutionSpawn.Invoke();
    }

    void OnEnable(){
        StartCoroutine(InitMovement());
    }

    IEnumerator InitMovement()
    {
        healthBar.SetActive(true);
        moveLeft = false;
        moveRight = false;
        followPlayer = false;
        lookRight = false;
        cooldown = false;
        freeze = true;
        UpdateLook();
        yield return new WaitForSeconds(3.5f);
        StartMovement();
        freeze = false;
    }

    void FixedUpdate()
    {
        healthSlider.value = health;
        AttackSystem();
        Movement();
    }

    private void Movement(){
        if (followPlayer)
        {
            UpdateLook();
            
            if (Mathf.Abs(distance) > 7)
            {
                if (lookRight)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x - limit, transform.position.y, transform.position.z), 15 * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x + limit, transform.position.y, transform.position.z), 15 * Time.deltaTime);
            } 
        }
        else if (moveLeft && !freeze)
        {
            MoveLeft();
        }
        else if (moveRight && !freeze)
        {
            MoveRight();
        }
    }

    private void AttackSystem()
    {
        if (!cooldown && !freeze)
        {
            cooldown = true;
            int rand = new System.Random().Next(0, 3);
            if (rand == 0)
            {
                StartCoroutine(LaserAttack());
            }
            else if (rand == 1)
            {
                StartCoroutine(DoAttack("Fist"));
            }
            else if (rand == 2)
            {
                StartCoroutine(DoAttack("Mouth"));
            }
        }
    }

    private void UpdateLook()
    {
        distance = target.transform.position.x - transform.position.x;

        if (distance > 0 && lookRight) // p x
        {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (distance < 0 && !lookRight) // x q
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (distance > 0 && !lookRight) // q x
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = true;
        }
        else if (distance < 0 && lookRight) // x p
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            lookRight = false;
        }
    }
        
    // Movement
    private void MoveLeft()
    {
        rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
        lookRight = false;
    }

    private void MoveRight()
    {
        rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
        lookRight = true;
    }

    private void StartMovement()
    {
        transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        moveLeft = true;
        moveRight = false; 
    }

    private void StopMovement()
    {
        moveLeft = false;
        moveRight = false;
    }

    //Attacks

    private IEnumerator LaserAttack()
    {
        StopMovement();
        ogreEvAnimation.SetBool("Eyes", true);
        yield return new WaitForSeconds(1.5f);

        StartMovement();
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        yield return new WaitForSeconds(6f);

        ogreEvAnimation.SetBool("Eyes", false);
        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        StartCoroutine(WaitCooldown());
    }

    IEnumerator DoAttack(string name)
    {
        StopMovement();
        followPlayer = true;
        ogreEvAnimation.SetTrigger(name);
        yield return new WaitForSeconds(2f);
        
        if (!freeze)
            StartMovement();
        followPlayer = false;
        StartCoroutine(WaitCooldown());
    } 

    public void InvokeLava(){
        lava.Play();
    }

    IEnumerator WaitCooldown()
    {
        yield return new WaitForSeconds(new System.Random().Next(timeCooldown-1, timeCooldown+1));
        cooldown = false;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        pre.SetActive(true);
        gameObject.SetActive(false);
        OnEvolutionSpawn.Invoke();
    }

    private void TakeDamage(int damage){
        health = health - damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        freeze = true;
        Instantiate(implosion, transform.position, Quaternion.identity);
        Instantiate(death, transform.position, Quaternion.identity);
        Destroy(gameObject, 1.5f);
        healthBar.SetActive(false);
        OpenSecondScreen();
    } 

    private void OpenSecondScreen()
    {
        jumpPad.SetActive(false);
        door.SetActive(false);
        floor6.SetActive(true);
    }

    // Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!followPlayer)
        {
            if (collision.gameObject.tag == "WallLeft")
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                moveLeft = false;
                moveRight = true;
            }

            if (collision.gameObject.tag == "WallRight")
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                moveLeft = true;
                moveRight = false;
            }
        }

        if (collision.gameObject.tag == "WeaponSoft")
        {
            TakeDamage(damage);
        }
        else if (collision.gameObject.tag == "PlayerDeath")
        {
            freeze = true;
            StartCoroutine(Respawn());
        }
    }
}
