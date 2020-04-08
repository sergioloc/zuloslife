using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    public Joystick joystick;
    public float speed = 0;
    public bool facingRight = true;
    private float scale;

    [Header("Jump")]
    public int jumpForce = 0;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Particles")]
    public ParticleSystem bloodParticle;
    public ParticleSystem confuseParticle;
    public ParticleSystem healthParticle;
    public ParticleSystem spawnParticle;

    [Header("Damage")]
    public GameObject deathEffect;
    public GameObject bloodEffect1;
    public GameObject bloodEffect2;
    public GameObject frame;

    [Header("Attack")]
    public GameObject megaCombo;
    public GameObject projectile;
    public Transform shotPoint;

    [Header("Stamina")]
    public Image staminaPanda;
    public Image staminaCinamon;
    public Image staminaKutter;
    public Image staminaTrisky;

    [Header("Health")]
    public Slider healthBar;
    public int damageFromEnemy = 10;
    public int initialHealth = 100;
    public float health;
    public GameObject deathCollider;

    [Header("Impact Faces")]
    private GameObject impactFace;
    public GameObject pandaImpactFace;
    public GameObject keroImpactFace;
    public GameObject cinamonImpactFace;
    public GameObject kutterImpactFace;
    public GameObject triskyImpactFace;

    [Header("Icons")]
    public Image fillPanda;
    public Image fillKero;
    public Image fillCinamon;
    public Image fillKutter;
    public Image fillTrisky;
    public Image fillCurrent;

    private Transform spawnPoint;
    private Rigidbody2D rb2d;
    private Animator animPlayer, cameraAnimation;
    private float sensitivity = 0.1f;
    private bool bloodShowed = false, wallAtRight, wallAtLeft, isInWater = false;

    public static PlayerController instance;
    public bool confuse = false, keyboard = false;

    public GameObject pandaGameObject, keroGameObject, cinamonGameObject, kutterGameObject,triskyGameObject;

    private Character panda, kero, cinamon, kutter, trisky, current;

    #endregion

    void Start()
    {
        instance = this;
        health = initialHealth;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        wallAtRight = false;
        wallAtLeft = false;
        panda = new Character("Panda", pandaGameObject, pandaImpactFace, fillPanda, staminaPanda);
        kero = new Character("Kero", keroGameObject, keroImpactFace, fillKero, null);
        cinamon = new Character("Cinamon", cinamonGameObject, cinamonImpactFace, fillCinamon, staminaCinamon);
        kutter = new Character("Kutter", kutterGameObject, kutterImpactFace, fillKutter, staminaKutter);
        trisky = new Character("Trisky", triskyGameObject, triskyImpactFace, fillTrisky, staminaTrisky);
        current = new Character();
        current = panda;
        current.SetIconColor(new Color32(0, 80, 255, 255));
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        healthBar.value = health;

        if (isInWater){
            Swimming();
        }
        else if (confuseParticle.isPlaying){
            ConfuseMovement();
        }
        else {
            Movement();
        }
        

        //Jump
        if (isGrounded)
            animPlayer.SetBool("isJumping", false);
        else
            animPlayer.SetBool("isJumping", true);

        //Combo
        if (megaCombo.activeInHierarchy == true)
        {
            CameraController.instance.Shake(0);
        }
    
    }


    // Movement

    private void Movement()
    {
        if (joystick.Horizontal >= sensitivity && !wallAtRight)
        {
            //Move Right
            current.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!facingRight) Flip();
        }
        else if (joystick.Horizontal <= -sensitivity && !wallAtLeft)
        {
            //Move Left
            current.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (facingRight) Flip();
        }
        else if (current.GetBool("Run"))
        {
            current.SetBool("Run", false);
        }
    }

    private void ConfuseMovement(){
        if ((joystick.Horizontal >= sensitivity) && !wallAtLeft)
        {
            //Move Left
            current.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (facingRight) Flip();
        }
        else if ((joystick.Horizontal <= -sensitivity) && !wallAtRight)
        {
            //Move Right
            current.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!facingRight) Flip();
        }
        else if (current.GetBool("Run"))
        {
            current.SetBool("Run", false);
        }
    }

    private void Swimming(){
        if (joystick.Horizontal >= (sensitivity * 5) && !wallAtRight)
            {
                //Move Right
                rb2d.transform.Translate(Vector2.right * speed * 0.75f * Time.deltaTime);
                if (!facingRight) Flip();
            }
            else if (joystick.Horizontal <= -(sensitivity * 5) && !wallAtLeft)
            {
                //Move Left
                rb2d.transform.Translate(Vector2.left * speed * 0.75f * Time.deltaTime);
                if (facingRight) Flip();
            }
            else if (joystick.Vertical >= sensitivity)
            {
                //Move Up
                rb2d.transform.Translate(Vector2.up * speed * 0.5f * Time.deltaTime);
            }
            else if (joystick.Vertical <= -sensitivity)
            {
                //Move Down
                rb2d.transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
            }
    }

    public void Jump()
    {
        if (!isInWater && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jumpForce * 100);
        }
    }


    // Action
    public void startAction()
    {
        if (current.GetStaminaFillAmount() >= 1)
        {
            current.SetBool("Action", true);

            //Restore health
            if (current.CompareNameTo("Trisky"))
            {
                if (health >= (initialHealth - 30))
                {
                    health = initialHealth;
                }
                else
                {
                    health = health + 30;
                }
                healthParticle.Play();
            }
            
            //Sound explosion
            else if (current.CompareNameTo("Cinamon") && isGrounded)
            {
                rb2d.AddForce(Vector2.up * jumpForce * 20);
                rb2d.gravityScale = 0.4f;
                StartCoroutine(Wait("RestoreGravity", 2f));
                CameraController.instance.Shake(0.7f);
            }

            //Throw scissor
            else if (current.CompareNameTo("Kutter"))
            {
                StartCoroutine(Wait("ThrowScissor", 0.3f));
            }
            
        }
        
    }

    public void stopAction()
    {
        current.SetBool("Action", false);
    }


    // Collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MeleeEnemy") && !bloodShowed)
        {
            TakeDamage(damageFromEnemy);
        }
        else if (collision.gameObject.CompareTag("Missile"))
        {
            TakeDamage(damageFromEnemy);
        }
        else if (collision.gameObject.tag == "OgreFist")
        {
            TakeDamage(10);
        }
        else if (collision.gameObject.tag == "OgreQuake")
        {
            TakeDamage(20);
        }
        else if (collision.gameObject.tag == "Respawn")
        {
            spawnPoint = collision.gameObject.transform;
        }
        else if (collision.gameObject.CompareTag("BossScreen"))
        {
            CameraController.instance.ModifyZoom(10f);
        }
        else if (collision.gameObject.CompareTag("WallLeft"))
        {
            wallAtLeft = true;
        }
        else if (collision.gameObject.CompareTag("WallRight"))
        {
            wallAtRight = true;
        }
        else if (collision.gameObject.CompareTag("Fox"))
        {
            confuseParticle.Play();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            CameraController.instance.ModifyZoom(10f);
            rb2d.gravityScale = -0.004f;
            isInWater = true;
        }
        else if (collision.gameObject.tag == "Laser")
        {
            TakeDamage(0.1f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            rb2d.gravityScale = 4;
            isInWater = false;
        }
        else if (collision.gameObject.CompareTag("WallLeft"))
        {
            wallAtLeft = false;
        }
        if (collision.gameObject.CompareTag("WallRight"))
        {
            wallAtRight = false;
        }
    }


    // Aux functions
    private void TakeDamage(float damage)
    {
        frame.SetActive(true);
        StartCoroutine(Wait("HideFrame", 0.5f));
        if (health > 0)
        {
            health = health - damage;
            bloodParticle.Play();
            if (!impactFace.gameObject.activeSelf)
            {
                impactFace.SetActive(true);
                StartCoroutine(Wait("ImpactFaceFalse", 2f));
            }
            if (health <= 0)
                Die();
        }
    }

    private void Flip()
    {
        if (facingRight)
        {
            transform.localScale = new Vector3(-scale, scale, scale);
        }
        else
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }
        shotPoint.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    private void Die()
    {
        if (!bloodShowed)
        {
            current.GetCharacter().SetActive(false);
            confuseParticle.Stop();
            deathCollider.SetActive(true);
            impactFace.SetActive(false);
            Instantiate(deathEffect, transform.position, Quaternion.identity);

            if (Random.Range(1, 2) == 1)
            {
                Instantiate(bloodEffect1, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(bloodEffect2, transform.position, Quaternion.identity);
            }
            bloodShowed = true;
            StartCoroutine(Wait("MoveToSpawnPoint", 2f));
        } 
    }

    IEnumerator Wait(string type, float sec)
    {
        yield return new WaitForSeconds(sec);
        switch (type)
        {
            case "ImpactFaceFalse":
                impactFace.SetActive(false);
                break;

            case "RestoreGravity":
                rb2d.gravityScale = 3f;
                break;

            case "MoveToSpawnPoint":
                deathCollider.SetActive(false);
                gameObject.GetComponent<Transform>().position = spawnPoint.position;
                spawnParticle.Play();
                gameObject.layer = 8;
                StartCoroutine(Wait("Respawn", 3f));
                break;

            case "Respawn":
                current.GetCharacter().SetActive(true);
                health = initialHealth;
                bloodShowed = false;
                break;

            case "ThrowScissor":
                Instantiate(projectile, shotPoint.position, shotPoint.rotation);
                break;

            case "HideFrame":
                frame.SetActive(false);
                break;
        }
    }


    // Switch character
    public void switchToPanda(){
        switchTo(panda);
    }

    public void switchToKero(){
        switchTo(kero);
    }

    public void switchToCinamon(){
        switchTo(cinamon);
    }

    public void switchToKutter(){
        switchTo(kutter);
    }

    public void switchToTrisky(){
        switchTo(trisky);
    }

    private void switchTo(Character next){
        current.Hide();
        current = next;
        current.Show();
    }


    // Keyboard
    private void KeyboardJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void KeyboardAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            startAction();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            stopAction();
        }
    }

    private void MovementKeyboard()
    {
        if (isInWater)
        {
            if (Input.GetKey(KeyCode.RightArrow) && !wallAtRight)
            {
                //Move Right
                rb2d.transform.Translate(Vector2.right * speed * 0.75f * Time.deltaTime);
                if (!facingRight) Flip();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtLeft)
            {
                //Move Left
                rb2d.transform.Translate(Vector2.left * speed * 0.75f * Time.deltaTime);
                if (facingRight) Flip();
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //Move Up
                rb2d.transform.Translate(Vector2.up * speed * 0.5f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                //Move Down
                rb2d.transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
            }
        }
        else   //Normal
        {
            if (!confuseParticle.isPlaying)
            {
                if (Input.GetKey(KeyCode.RightArrow) && !wallAtRight)
                {
                    //Move Right
                    animPlayer.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    if (!facingRight) Flip();
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtLeft)
                {
                    //Move Left
                    animPlayer.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    if (facingRight) Flip();
                }
                else if (animPlayer.GetBool("Run"))
                {
                    animPlayer.SetBool("Run", false);
                }
            }

            else //Confuse
            {
                if (Input.GetKey(KeyCode.RightArrow) && !wallAtLeft)
                {
                    //Move Left
                    animPlayer.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    if (facingRight) Flip();
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtRight)
                {
                    //Move Right
                    animPlayer.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    if (!facingRight) Flip();
                }
                else if (animPlayer.GetBool("Run"))
                {
                    animPlayer.SetBool("Run", false);
                }
            }
        }
    }

}

