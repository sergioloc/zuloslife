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
    public int speed = 0;
    public bool facingRight = true;
    private float scale;

    [Header("Jump")]
    public int jumpForce = 0;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public int extraJumpsValue;
    private bool isGrounded;
    private int extraJumps;

    [Header("Particles")]
    public ParticleSystem bloodParticle;
    public ParticleSystem confuseParticle;
    public ParticleSystem healthParticle;
    public ParticleSystem spawnParticle;

    [Header("Die")]
    public GameObject deathEffect;
    public GameObject bloodEffect1;
    public GameObject bloodEffect2;

    [Header("Attack")]
    public GameObject flashCollider;
    public GameObject megaCombo;
    public GameObject projectile;
    public Transform shotPoint;

    [Header("Stamina")]
    public Image staminaPanda;
    public Image staminaCinamon;
    public Image staminaKutter;
    public Image staminaTrisky;

    [Header("Impact Faces")]
    private GameObject impactFace;
    public GameObject pandaImpactFace;
    public GameObject keroImpactFace;
    public GameObject cinamonImpactFace;
    public GameObject kutterImpactFace;
    public GameObject triskyImpactFace;

    [Header("Health")]
    public Slider healthBar;
    public int damageFromEnemy = 10;
    public int initialHealth = 100;
    public int health;
    public GameObject deathCollider;

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;

    private Transform spawnPoint;
    private Rigidbody2D rb2d;
    private Animator characterAnimation, cameraAnimation;
    private float sensitivity = 0.1f;
    private bool bloodShowed = false, shootActive = true, collisionWallRight = false, collisionWallLeft = false, isInWater = false;

    public static PlayerController instance;

    #endregion


    void Start()
    {
        instance = this;
        health = initialHealth;
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        characterAnimation = panda.GetComponent<Animator>();
        impactFace = pandaImpactFace;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        healthBar.value = health;

        if (characters.gameObject.activeInHierarchy)
        {
            //Move
            if (collisionWallRight)
                Movement(false, true);
            else if (collisionWallLeft)
                Movement(true, false);
            else
                Movement(true, true);


            //Jump
            if (isGrounded)
            {
                extraJumps = extraJumpsValue;
                characterAnimation.SetBool("isJumping", false);
            }
            else
            {
                characterAnimation.SetBool("isJumping", true);
            }

            //Throw scissor
            if (shootActive && shotPoint.gameObject.activeSelf)
            {
                Instantiate(projectile, shotPoint.position, shotPoint.rotation);
                shootActive = false;
                StartCoroutine(Wait("ActiveShoot", 0.9f));
            }

            //Die
            if (health <= 0)
            {
                Die();
            }

            //Keyboard controllers
            KeyboardSwitch();
            KeyboardJump();
            KeyboardAction();

            //Combo
            if (megaCombo.activeInHierarchy == true)
            {
                CameraController.instance.Shake(0);
            }
        }

    }

    #region Movement

    private void Movement(bool right, bool left)
    {
        if (!confuseParticle.isPlaying)
        {
            if ((joystick.Horizontal >= sensitivity || Input.GetKey(KeyCode.RightArrow)) && right)
            {
                MoveRight();
            }
            else if ((joystick.Horizontal <= -sensitivity || Input.GetKey(KeyCode.LeftArrow)) && left)
            {
                MoveLeft();
            }
            else if (characterAnimation.GetBool("Run"))
            {
                characterAnimation.SetBool("Run", false);
            }
        }

        else //If is confuse
        {
            if ((joystick.Horizontal >= sensitivity || Input.GetKey(KeyCode.RightArrow)) && left)
            {
                MoveLeft();
            }
            else if ((joystick.Horizontal <= -sensitivity || Input.GetKey(KeyCode.LeftArrow)) && right)
            {
                MoveRight();
            }
            else if (characterAnimation.GetBool("Run"))
            {
                characterAnimation.SetBool("Run", false);
            }
        }

        if (isInWater)
        {
            //Joystick
            if (joystick.Vertical >= sensitivity)
            {
                rb2d.transform.Translate(Vector2.up * speed * 0.5f * Time.deltaTime);
            }
            else if (joystick.Vertical <= -sensitivity)
            {
                rb2d.transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
            }
            //Keyboard
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.transform.Translate(Vector2.up * speed * 0.5f * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rb2d.transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
            }
        }

    }

    private void MoveRight()
    {
        characterAnimation.SetBool("Run", true);
        rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (!facingRight) Flip();
    }

    private void MoveLeft()
    {
        characterAnimation.SetBool("Run", true);
        rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (facingRight) Flip();
    }

    public void Jump()
    {
        if (!isInWater)
        {
            if (extraJumps > 0)
            {
                rb2d.AddForce(Vector2.up * jumpForce * 100);
                extraJumps--;
            }
            else if (extraJumps == 0 && isGrounded)
            {
                rb2d.AddForce(Vector2.up * jumpForce * 100);
            }
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
                health = health + 30;
                healthParticle.Play();
                StartCoroutine(Wait("StopHealthParticle", 1.5f));
            }
            //Sound explosion
            else if (currentCharacter == "cinamon" && isGrounded)
            {
                rb2d.AddForce(Vector2.up * jumpForce * 20);
                rb2d.gravityScale = 0.4f;
                StartCoroutine(Wait("RestoreGravity", 2f));
                CameraController.instance.Shake(0.7f);
            }
        }
        
    }

    public void stopAction()
    {
        characterAnimation.SetBool("Action", false);
    }
    #endregion

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MeleeEnemy") && !bloodShowed)
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
        else if (collision.gameObject.tag == "Laser")
        {
            TakeDamage(30);
        }
        else if (collision.gameObject.tag == "Respawn")
        {
            spawnPoint = collision.gameObject.transform;
        }
        else if (collision.gameObject.CompareTag("BossScreen"))
        {
            CameraController.instance.ModifyZoom(10f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            virtualCamera.m_Lens.OrthographicSize = 10;
            rb2d.gravityScale = -0.002f;
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            rb2d.gravityScale = 4;
            isInWater = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fox"))
        {
            confuseParticle.Play();
        }
        else if (collision.gameObject.CompareTag("WallRight"))
        {
            collisionWallRight = true;
        }
        else if (collision.gameObject.CompareTag("WallLeft"))
        {
            collisionWallLeft = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WallRight"))
        {
            collisionWallRight = false;
        }
        else if (collision.gameObject.CompareTag("WallLeft"))
        {
            collisionWallLeft = false;
        }
    }

    #endregion

    #region Other functions

    private void TakeDamage(int damage)
    {
        health = health - damage;
        bloodParticle.Play();
        if (!impactFace.gameObject.activeSelf)
        {
            impactFace.SetActive(true);
            StartCoroutine(Wait("ImpactFaceFalse", 2f));
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

            case "StopHealthParticle":
                healthParticle.Stop();
                break;

            case "RestoreGravity":
                rb2d.gravityScale = 3f;
                break;

            case "MoveToSpawnPoint":
                deathCollider.SetActive(false);
                gameObject.GetComponent<Transform>().position = spawnPoint.position;
                spawnParticle.Play();
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

            case "ActiveShoot":
                shootActive = true;
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
    }
    #endregion

}

