using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Characters")]
    public GameObject kero;
    public GameObject panda;
    public GameObject cinamon;
    public GameObject kutter;
    public GameObject trisky;
    public GameObject myCamera;
    [Tooltip("Initial character")]
    public string currentCharacter;

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

    [Space]
    public GameObject flashCollider;
    public ParticleSystem bloodParticle;
    public GameObject weapon, projectile;
    public Transform shotPoint;

    private Rigidbody2D rb2d;
    private Animator characterAnimation, cameraAnimation;
    private GameObject impactFace;
    public GameObject keroImpactFace, cinamonImpactFace, kutterImpactFace, triskyImpactFace;

    private bool shootActive = true;


    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb2d = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
        cameraAnimation = myCamera.GetComponent<Animator>();
        //keroImpactFace = GameObject.Find("Player/Kero/KeroBody/bone_pants/bone_chest/bone_head/face_impact");
        //cinamonImpactFace = GameObject.Find("Player/Cinamon/CinamonBody/bone_pants/bone_chest/bone_head/Cinamon_Face_Impact");
        //kutterImpactFace = GameObject.Find("Player/Kutter/KutterBody/bone_pants/bone_chest/bone_head/Kutter_Face_Impact");
        //triskyImpactFace = GameObject.Find("Player/Trisky/TriskyBody/bone_pants/bone_chest/bone_head/Trisky_Face_Impact");
        //characterAnimation = trisky.GetComponent<Animator>();
        impactFace = triskyImpactFace;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Movement
        if (joystick.Horizontal >= 0.1f)
        {
            characterAnimation.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!facingRight) Flip();
        }
        else if (joystick.Horizontal <= -0.1f)
        {
            characterAnimation.SetBool("Run", true);
            rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (facingRight) Flip();
        }
        else
        {
            characterAnimation.SetBool("Run", false);
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

        //Temp
        if (Input.GetKeyDown(KeyCode.G))
        {
            startAction();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            stopAction();
        }
    }

    public void Jump()
    {
        GetComponent<Rigidbody2D>().gravityScale = 3f;
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
        
        if (currentCharacter == "cinamon" && isGrounded)
        {
            rb2d.AddForce(Vector2.up * jump * 40);
            GetComponent<Rigidbody2D>().gravityScale = 0.4f;
            cameraAnimation.SetTrigger("Shake");
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
            push();
            if (currentCharacter == "cinamon")
            {
                GetComponent<Rigidbody2D>().gravityScale = 3f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponEnemy"))
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        impactFace.SetActive(false);
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
        rb2d.AddForce(new Vector3(750, 900));
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
        currentCharacter = "panda";
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
    }
    #endregion


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
}

