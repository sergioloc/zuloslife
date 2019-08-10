using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Characters")]
    public GameObject kero;
    public GameObject panda;
    public GameObject cinamon;
    public GameObject kutter;
    public GameObject trisky;
    [Tooltip("Initial character")]
    public string currentCharacter;

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
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

    [Header("Die")]
    public GameObject deathEffect;
    public GameObject bloodEffect1;
    public GameObject bloodEffect2;



    [Space]
    public GameObject flashCollider, megaCombo;
    public GameObject weapon, projectile;
    public Transform shotPoint;

    private Rigidbody2D rb2d;
    private Animator characterAnimation, cameraAnimation;
    private GameObject impactFace;
    public GameObject pandaImpactFace, keroImpactFace, cinamonImpactFace, kutterImpactFace, triskyImpactFace;

    private bool shootActive = true;
    private int health;
    private float sensitivity = 0.1f;
    public Slider healthBar;
    private bool waitShake = true;





    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        //keroImpactFace = GameObject.Find("Player/Kero/KeroBody/bone_pants/bone_chest/bone_head/face_impact");
        //cinamonImpactFace = GameObject.Find("Player/Cinamon/CinamonBody/bone_pants/bone_chest/bone_head/Cinamon_Face_Impact");
        //kutterImpactFace = GameObject.Find("Player/Kutter/KutterBody/bone_pants/bone_chest/bone_head/Kutter_Face_Impact");
        //triskyImpactFace = GameObject.Find("Player/Trisky/TriskyBody/bone_pants/bone_chest/bone_head/Trisky_Face_Impact");
        characterAnimation = cinamon.GetComponent<Animator>();
        impactFace = pandaImpactFace;
        health = 100;

        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
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

        if (confuseParticle.isPlaying)
        {
            //Confuse Movement
            if (joystick.Horizontal >= sensitivity)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                if (facingRight) Flip();
            }
            else if (joystick.Horizontal <= -sensitivity)
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
        else
        {
            //Movement
            if (joystick.Horizontal >= sensitivity)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                if (!facingRight) Flip();
            }
            else if (joystick.Horizontal <= -sensitivity)
            {
                characterAnimation.SetBool("Run", true);
                rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                if (facingRight) Flip();
            }
            else if(characterAnimation.GetBool("Run"))
            {
                characterAnimation.SetBool("Run", false);
            }
        }   

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

        //Die
        if(health == 0)
        {
            Die();
        }

        

    }


    public void Jump()
    {
        if (extraJumps > 0)
        {
            characterAnimation.SetTrigger("TakeOff");
            rb2d.AddForce(Vector2.up * jump * 100);
            extraJumps--;
        }
        else if (extraJumps == 0 && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jump);
        }
    }

    #region Action
    public void startAction()
    {
        characterAnimation.SetBool("Action", true);

        if(currentCharacter == "trisky")
        {
            health = health + 30;
            healthParticle.Play();
            StartCoroutine(WaitForStopHealth());
        }
        
        if (currentCharacter == "cinamon" && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jump * 40);
            GetComponent<Rigidbody2D>().gravityScale = 0.4f;
            StartCoroutine(WaitForGravity());
            waitShake = true;
            shakeElapsedTime = shakeDuration;
        }
        else if(currentCharacter == "kutter")
        {
           
            if(shootActive)
            {
                StartCoroutine(throwScissor());
                shootActive = false;
                StartCoroutine(activeShoot());
            }
        }

    }

    private void ShakeCamera()
    {
        // If the Cinemachine componet is not set, avoid update
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (shakeElapsedTime > 0)
            {
                if(waitShake)
                    StartCoroutine(WaitForShake(0.7f));
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

    public void stopAction()
    {
        characterAnimation.SetBool("Action", false);

        if (currentCharacter == "kero")
        {
            weapon.SetActive(false);
        }

    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponEnemy"))
        {
            bloodParticle.Play();
            impactFace.SetActive(true);
            health = health - 5;
            push();
        }

        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponEnemy"))
        {
            StartCoroutine(Wait(3));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fox"))
        {
            confuseParticle.Play();
        }
    }

    IEnumerator Wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        impactFace.SetActive(false);
    }

    IEnumerator WaitForShake(float sec)
    {
        yield return new WaitForSeconds(sec);
        // Set Cinemachine Camera Noise parameters
        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = shakeFrequency;
    }

    IEnumerator WaitForStopHealth()
    {
        yield return new WaitForSeconds(1.5f);
        healthParticle.Stop();
    }

    IEnumerator WaitForGravity()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody2D>().gravityScale = 3f;
    }

    IEnumerator throwScissor()
    {
        yield return new WaitForSeconds(0.35f);
        Instantiate(projectile, shotPoint.position, shotPoint.rotation);
    }

    IEnumerator activeShoot()
    {
        yield return new WaitForSeconds(0.9f);
        shootActive = true;
    }

    private void push()
    {
        if(facingRight)
            rb2d.AddForce(new Vector3(-750, 900));
        else
            rb2d.AddForce(new Vector3(-750, 900));
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

