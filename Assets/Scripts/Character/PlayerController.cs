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

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineComposer virtualComposer;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;
    private float shakeElapsedTime = 0f;

    [Header("Move Controller")]
    public Joystick joystick;
    public int speed = 0;
    private bool facingRight = true;
    private float scale;

    [Header("Jump Controller")]
    public int jump = 0;
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
    public GameObject pandaImpactFace;
    public GameObject keroImpactFace;
    public GameObject cinamonImpactFace;
    public GameObject kutterImpactFace;
    public GameObject triskyImpactFace;

    [Space]
    public Slider healthBar;
    public int damageFromEnemy = 10;
    public GameObject deathCollider;

    private Transform spawnPoint;
    private bool bloodShowed = false;
    private Rigidbody2D rb2d;
    private Animator characterAnimation, cameraAnimation;
    private GameObject impactFace;
    private bool shootActive = true;
    public int health;
    private float sensitivity = 0.1f;
    private bool waitShake = true;
    private bool collisionWallRight = false, collisionWallLeft = false, isInWater = false;

    public static PlayerController instance;

    #endregion


    void Start()
    {
        instance = this;
        health = 100;
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        //keroImpactFace = GameObject.Find("Player/Kero/KeroBody/bone_pants/bone_chest/bone_head/face_impact");
        //cinamonImpactFace = GameObject.Find("Player/Cinamon/CinamonBody/bone_pants/bone_chest/bone_head/Cinamon_Face_Impact");
        //kutterImpactFace = GameObject.Find("Player/Kutter/KutterBody/bone_pants/bone_chest/bone_head/Kutter_Face_Impact");
        //triskyImpactFace = GameObject.Find("Player/Trisky/TriskyBody/bone_pants/bone_chest/bone_head/Trisky_Face_Impact");
        characterAnimation = panda.GetComponent<Animator>();
        impactFace = pandaImpactFace;
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        virtualComposer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        healthBar.value = health;

        //Combo
        if (megaCombo.activeInHierarchy == true)
        {
            waitShake = false;
            shakeElapsedTime = shakeDuration;
            Debug.Log("MegaCombo");
        }

        ShakeCamera();

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

            //Change Character in Keyboard
            /*
            if (Input.GetKeyDown(KeyCode.UpArrow))
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
            else if (Input.GetKeyDown(KeyCode.DownArrow))
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
            */
        }

    }

    private void Movement(bool right, bool left)
    {
        if (!confuseParticle.isPlaying)
        {
            if ((joystick.Horizontal >= sensitivity || Input.GetKey(KeyCode.RightArrow)) && right)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                if (!facingRight) Flip();
            }
            else if ((joystick.Horizontal <= -sensitivity || Input.GetKey(KeyCode.LeftArrow)) && left)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                if (facingRight) Flip();
            }
            else if (characterAnimation.GetBool("Run"))
            {
                characterAnimation.SetBool("Run", false);
            }
        }

        else
        {
            if ((joystick.Horizontal >= sensitivity || Input.GetKey(KeyCode.RightArrow)) && left)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                if (facingRight) Flip();
            }
            else if ((joystick.Horizontal <= -sensitivity || Input.GetKey(KeyCode.LeftArrow)) && right)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                if (!facingRight) Flip();
            }
            else if (characterAnimation.GetBool("Run"))
            {
                characterAnimation.SetBool("Run", false);
            }
        }

        if (isInWater)
        {
            if (joystick.Vertical >= sensitivity)
            {
                rb2d.transform.Translate(Vector2.up * speed * 0.5f * Time.deltaTime);
            }
            else if (joystick.Vertical <= -sensitivity)
            {
                rb2d.transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
            }
        }
        

        //Keyboard jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            rb2d.transform.Translate(Vector2.up * speed * 0.5f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2d.transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            startAction();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            stopAction();
        }

    }




    public void Jump()
    {
        if (!isInWater)
        {
            if (extraJumps > 0)
            {
                characterAnimation.SetTrigger("TakeOff");
                rb2d.AddForce(Vector2.up * jump * 100);
                extraJumps--;
            }
            else if (extraJumps == 0 && isGrounded)
            {
                rb2d.AddForce(Vector2.up * jump * 100);
            }
        }
    }

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

            if (currentCharacter == "trisky") //Restore health
            {
                health = health + 30;
                healthParticle.Play();
                StartCoroutine(Wait("StopHealthParticle", 1.5f));
            }
            else if (currentCharacter == "cinamon" && isGrounded) //Sound explosion
            {
                rb2d.AddForce(Vector2.up * jump * 20);
                rb2d.gravityScale = 0.4f;
                StartCoroutine(Wait("RestoreGravity", 2f));
                waitShake = true;
                shakeElapsedTime = shakeDuration;
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
            //Push(750, 900);
        }
        else if (collision.gameObject.CompareTag("BossScreen"))
        {
            virtualCamera.m_Lens.OrthographicSize = 10;
        }
        else if (collision.gameObject.tag == "OgreFist")
        {
            TakeDamage(10);
            //Push(6000, 2000);
        }
        else if (collision.gameObject.tag == "OgreQuake")
        {
            TakeDamage(20);
            //Push(0, 1300);
        }
        else if (collision.gameObject.tag == "Laser")
        {
            TakeDamage(30);
            //Push(0, 1500);
        }
        else if (collision.gameObject.tag == "Respawn")
        {
            spawnPoint = collision.gameObject.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "OgreQuake")
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.tag == "Water")
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

    void OnCollisionEnter2D(Collision2D collision)
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

    private void Push(int strong, int high)
    {
        if(facingRight)
            rb2d.AddForce(new Vector3(-strong, high));
        else
            rb2d.AddForce(new Vector3(strong, high));
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
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

    private void ShakeCamera()
    {
        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (shakeElapsedTime > 0)
            {
                if (waitShake)
                    StartCoroutine(Wait("ShakeScreen", 0.7f));
                else
                {
                    virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                    virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                }
                // Update Shake Timer
                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
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

            case "ShakeScreen":
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
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
                //gameObject.GetComponent<CapsuleCollider2D>().tag = "Player";
                health = 100;
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
            //gameObject.GetComponent<CapsuleCollider2D>().tag = "Ignore";

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

