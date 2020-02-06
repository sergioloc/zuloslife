using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Characters")]
    public GameObject kero;
    public GameObject panda;
    public GameObject cinamon;
    public GameObject kutter;
    public GameObject trisky;
    [Tooltip("Initial character")]
    public string currentCharacter;
    public GameObject characters;

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

    private Transform spawnPoint;
    private Rigidbody2D rb2d;
    private Animator characterAnimation, cameraAnimation;
    private float sensitivity = 0.1f;
    private bool bloodShowed = false, wallAtRight, wallAtLeft, isInWater = false;

    public static PlayerController instance;
    public bool confuse = false, keyboard = false;

    #endregion

    void Start()
    {
        instance = this;
        health = initialHealth;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        fillPanda.color = new Color32(0, 80, 255, 255);
        characterAnimation = panda.GetComponent<Animator>();
        impactFace = pandaImpactFace;
        wallAtRight = false;
        wallAtLeft = false;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        healthBar.value = health;

        if (characters.gameObject.activeInHierarchy)
        {
            if (keyboard)
            {
                MovementKeyboard();
                KeyboardSwitch();
                KeyboardJump();
                KeyboardAction();
            } 
            else
                MovementJoystick();

            //Jump
            if (isGrounded)
            {
                characterAnimation.SetBool("isJumping", false);
            }
            else
            {
                characterAnimation.SetBool("isJumping", true);
            }

            //Die
            if (health <= 0)
            {
                Die();
            }

            //Combo
            if (megaCombo.activeInHierarchy == true)
            {
                CameraController.instance.Shake(0);
            }

            //testing
            if (confuse)
            {
                confuse = false;
                confuseParticle.Play();
            }


        }

    }

    #region Movement

    private void MovementJoystick()
    {
        if (isInWater)
        {
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
        else   //Normal
        {
            if (!confuseParticle.isPlaying)
            {
                if (joystick.Horizontal >= sensitivity && !wallAtRight)
                {
                    //Move Right
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    if (!facingRight) Flip();
                }
                else if (joystick.Horizontal <= -sensitivity && !wallAtLeft)
                {
                    //Move Left
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    if (facingRight) Flip();
                }
                else if (characterAnimation.GetBool("Run"))
                {
                    characterAnimation.SetBool("Run", false);
                }
            }

            else //Confuse
            {
                if ((joystick.Horizontal >= sensitivity) && !wallAtLeft)
                {
                    //Move Left
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    if (facingRight) Flip();
                }
                else if ((joystick.Horizontal <= -sensitivity) && !wallAtRight)
                {
                    //Move Right
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    if (!facingRight) Flip();
                }
                else if (characterAnimation.GetBool("Run"))
                {
                    characterAnimation.SetBool("Run", false);
                }
            }
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
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    if (!facingRight) Flip();
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtLeft)
                {
                    //Move Left
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    if (facingRight) Flip();
                }
                else if (characterAnimation.GetBool("Run"))
                {
                    characterAnimation.SetBool("Run", false);
                }
            }

            else //Confuse
            {
                if (Input.GetKey(KeyCode.RightArrow) && !wallAtLeft)
                {
                    //Move Left
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                    if (facingRight) Flip();
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtRight)
                {
                    //Move Right
                    characterAnimation.SetBool("Run", true);
                    rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                    if (!facingRight) Flip();
                }
                else if (characterAnimation.GetBool("Run"))
                {
                    characterAnimation.SetBool("Run", false);
                }
            }
        }
    }

    public void Jump()
    {
        if (!isInWater && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jumpForce * 100);
        }
    }

    #endregion

    #region Action

    public void startAction()
    {
        bool cancelAction = false;

        if ((staminaPanda.fillAmount < 1 && currentCharacter == "panda") ||
            (staminaCinamon.fillAmount < 1 && currentCharacter == "cinamon") ||
            (staminaKutter.fillAmount < 1 && currentCharacter == "kutter") ||
            (staminaTrisky.fillAmount < 1 && currentCharacter == "trisky"))
        {
            cancelAction = true;
        }
        
        if (!cancelAction)
        {
            characterAnimation.SetBool("Action", true);

            //Restore health
            if (currentCharacter == "trisky")
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
            else if (currentCharacter == "cinamon" && isGrounded)
            {
                rb2d.AddForce(Vector2.up * jumpForce * 20);
                rb2d.gravityScale = 0.4f;
                StartCoroutine(Wait("RestoreGravity", 2f));
                CameraController.instance.Shake(0.7f);
            }
            //Throw scissor
            else if (currentCharacter == "kutter")
            {
                StartCoroutine(Wait("ThrowScissor", 0.3f));
            }
        }
        
    }

    public void stopAction()
    {
        characterAnimation.SetBool("Action", false);
    }
    #endregion

    #region Collisions

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


    #endregion

    #region Other functions

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
                characters.SetActive(true);
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

    private void Die()
    {
        if (!bloodShowed)
        {
            deathCollider.SetActive(true);
            impactFace.SetActive(false);
            characters.SetActive(false);
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
    #endregion

    #region Keyboard controllers

    private void KeyboardSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (currentCharacter == "panda")
                switchToKero();
            else if (currentCharacter == "kero")
                switchToCinamon();
            else if (currentCharacter == "cinamon")
                switchToKutter();
            else if (currentCharacter == "kutter")
                switchToTrisky();
            else if (currentCharacter == "trisky")
                switchToPanda();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

            if (currentCharacter == "panda")
                switchToTrisky();
            else if (currentCharacter == "kero")
                switchToPanda();
            else if (currentCharacter == "cinamon")
                switchToKero();
            else if (currentCharacter == "kutter")
                switchToCinamon();
            else if (currentCharacter == "trisky")
                switchToKutter();

        }
    }

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

    #endregion

    #region Switch Characters
    public void switchToPanda()
    {
        panda.SetActive(true);
        kero.SetActive(false);
        cinamon.SetActive(false);
        kutter.SetActive(false);
        trisky.SetActive(false);
        characterAnimation = panda.GetComponent<Animator>();
        impactFace = pandaImpactFace;
        currentCharacter = "panda";
        pandaImpactFace.SetActive(false);
        fillPanda.color = new Color32(0, 80, 255, 255);
        fillKero.color = new Color32(255, 255, 255, 255);
        fillCinamon.color = new Color32(255, 255, 255, 255);
        fillKutter.color = new Color32(255, 255, 255, 255);
        fillTrisky.color = new Color32(255, 255, 255, 255);
    }

    public void switchToKero()
    {
        panda.SetActive(false);
        kero.SetActive(true);
        cinamon.SetActive(false);
        kutter.SetActive(false);
        trisky.SetActive(false);
        characterAnimation = kero.GetComponent<Animator>();
        impactFace = keroImpactFace;
        currentCharacter = "kero";
        keroImpactFace.SetActive(false);
        fillPanda.color = new Color32(255, 255, 255, 255);
        fillKero.color = new Color32(0, 80, 255, 255);
        fillCinamon.color = new Color32(255, 255, 255, 255);
        fillKutter.color = new Color32(255, 255, 255, 255);
        fillTrisky.color = new Color32(255, 255, 255, 255);
    }

    public void switchToCinamon()
    {
        panda.SetActive(false);
        kero.SetActive(false);
        cinamon.SetActive(true);
        kutter.SetActive(false);
        trisky.SetActive(false);
        characterAnimation = cinamon.GetComponent<Animator>();
        impactFace = cinamonImpactFace;
        currentCharacter = "cinamon";
        cinamonImpactFace.SetActive(false);
        fillPanda.color = new Color32(255, 255, 255, 255);
        fillKero.color = new Color32(255, 255, 255, 255);
        fillCinamon.color = new Color32(0, 80, 255, 255);
        fillKutter.color = new Color32(255, 255, 255, 255);
        fillTrisky.color = new Color32(255, 255, 255, 255);
    }

    public void switchToKutter()
    {
        panda.SetActive(false);
        kero.SetActive(false);
        cinamon.SetActive(false);
        kutter.SetActive(true);
        trisky.SetActive(false);
        characterAnimation = kutter.GetComponent<Animator>();
        impactFace = kutterImpactFace;
        currentCharacter = "kutter";
        kutterImpactFace.SetActive(false);
        fillPanda.color = new Color32(255, 255, 255, 255);
        fillKero.color = new Color32(255, 255, 255, 255);
        fillCinamon.color = new Color32(255, 255, 255, 255);
        fillKutter.color = new Color32(0, 80, 255, 255);
        fillTrisky.color = new Color32(255, 255, 255, 255);
    }

    public void switchToTrisky()
    {
        panda.SetActive(false);
        kero.SetActive(false);
        cinamon.SetActive(false);
        kutter.SetActive(false);
        trisky.SetActive(true);
        characterAnimation = trisky.GetComponent<Animator>();
        impactFace = triskyImpactFace;
        currentCharacter = "trisky";
        triskyImpactFace.SetActive(false);
        fillPanda.color = new Color32(255, 255, 255, 255);
        fillKero.color = new Color32(255, 255, 255, 255);
        fillCinamon.color = new Color32(255, 255, 255, 255);
        fillKutter.color = new Color32(255, 255, 255, 255);
        fillTrisky.color = new Color32(0, 80, 255, 255);
    }
    #endregion

}

