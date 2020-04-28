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
    public ParticleSystem jumpParticle;
    public GameObject dustParticle;

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
    public Image staminaKero;
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
    private Animator cameraAnimation;
    private float sensitivity = 0.2f;
    private bool wallAtRight, wallAtLeft, isInWater = false, isJumping = false, isTakingPhoto = false;

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
        wallAtRight = false;
        wallAtLeft = false;
        panda = new Character("Panda", pandaGameObject, pandaImpactFace, fillPanda, staminaPanda);
        kero = new Character("Kero", keroGameObject, keroImpactFace, fillKero, staminaKero);
        cinamon = new Character("Cinamon", cinamonGameObject, cinamonImpactFace, fillCinamon, staminaCinamon);
        kutter = new Character("Kutter", kutterGameObject, kutterImpactFace, fillKutter, staminaKutter);
        trisky = new Character("Trisky", triskyGameObject, triskyImpactFace, fillTrisky, staminaTrisky);
        SetInitialCharacter();
        current.SetIconColor(new Color32(0, 80, 255, 255));
    }

    private void SetInitialCharacter(){
        if (pandaGameObject.activeInHierarchy)
            current = panda;
        else if (keroGameObject.activeInHierarchy)
            current = kero;
        else if (cinamonGameObject.activeInHierarchy)
            current = cinamon;
        else if (kutterGameObject.activeInHierarchy)
            current = kutter;
        else
            current = trisky;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        healthBar.value = health;

        if (!keyboard){
            if (isInWater){
                Swimming();
            }
            else if (confuseParticle.isPlaying){
                ConfuseMovement();
            }
            else {
                Movement();
            }
        }
        else{
            if (isInWater){
                SwimmingKeyboard();
            }
            else if (confuseParticle.isPlaying){
                ConfuseMovementKeyboard();
            }
            else {
                MovementKeyboard();
            }
            KeyboardAction();
            KeyboardJump();
        }
        
        //Jump
        if (isGrounded)
            current.SetBool("isJumping", false);
        else
            current.SetBool("isJumping", true);
    
    }


    // Movement

    private void Movement()
    {
        Debug.Log(joystick.Horizontal);
        if (joystick.Horizontal >= sensitivity && !wallAtRight)
        {
            RunTo("Right");
        }
        else if (joystick.Horizontal <= -sensitivity && !wallAtLeft)
        {
            RunTo("Left");
        }
        else if (current.GetBool("Run"))
        {
            StopRunning();
        }
    }

    private void ConfuseMovement(){
        if ((joystick.Horizontal >= sensitivity) && !wallAtLeft)
        {
            RunTo("Left");
        }
        else if ((joystick.Horizontal <= -sensitivity) && !wallAtRight)
        {
            RunTo("Right");
        }
        else if (current.GetBool("Run"))
        {
            StopRunning();
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
        if (!isInWater && isGrounded && !isJumping)
        {
            isJumping = true;
            jumpParticle.Play();
            rb2d.AddForce(Vector2.up * jumpForce * 100);
            StartCoroutine(EnableJump());
        }
    }

    private IEnumerator EnableJump(){
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
        StopCoroutine(EnableJump());
    }

    private void RunTo(string direction){
        if (!isTakingPhoto){
            current.SetBool("Run", true);
            if (isGrounded)
                dustParticle.SetActive(true);
            if (direction == "Right"){
                rb2d.transform.Translate(Vector2.right * speed * Time.deltaTime);
                if (!facingRight) Flip();
            }
            else{
                rb2d.transform.Translate(Vector2.left * speed * Time.deltaTime);
                if (facingRight) Flip();
            } 
        }  
    }

    private void StopRunning(){
        dustParticle.SetActive(false);
        current.SetBool("Run", false);
    }

    // Action
    public void startAction()
    {
        if (current.GetStaminaFillAmount() >= 1)
        {
            current.SetBool("Action", true);

            if (current.CompareNameTo("Kero")){
                current.SetBool("Action2", true);
                current.SetBool("Action3", true);
                /// FIXME: Shake
            }

            //Restore health
            else if (current.CompareNameTo("Trisky") && isGrounded)
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
                CameraController.instance.Shake(1f);
            }

            //Throw scissor
            else if (current.CompareNameTo("Kutter"))
            {
                StartCoroutine(Wait("ThrowScissor", 0.3f));
            }

            else if (current.CompareNameTo("Panda") && !current.GetBool("Run")){
                isTakingPhoto = true;
                StartCoroutine(NoTakingPhoto());
            }
            
        }
        
    }

    private IEnumerator NoTakingPhoto(){
        yield return new WaitForSeconds(0.9f);
        isTakingPhoto = false;
    }

    public void stopAction()
    {
        current.SetBool("Action", false);
        if (current.CompareNameTo("Kero")){
            current.SetBool("Action2", false);
            current.SetBool("Action3", false);
        }
    }


    // Collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MeleeEnemy"))
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
            if (!current.IsImpactFaceActive())
            {
                current.SetImpactFaceActive(true);
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
            transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
        }
        else
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        shotPoint.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    private void Die()
    {
        confuseParticle.Stop();
        healthParticle.Stop();
        current.GetCharacter().SetActive(false);
        deathCollider.SetActive(true);
        current.SetImpactFaceActive(false);
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (Random.Range(1, 2) == 1)
        {
            Instantiate(bloodEffect1, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(bloodEffect2, transform.position, Quaternion.identity);
        }
        StartCoroutine(Wait("MoveToSpawnPoint", 2f));
    }

    IEnumerator Wait(string type, float sec)
    {
        yield return new WaitForSeconds(sec);
        switch (type)
        {
            case "ImpactFaceFalse":
                current.SetImpactFaceActive(false);
                break;

            case "MoveToSpawnPoint":
                deathCollider.SetActive(false);
                gameObject.GetComponent<Transform>().position = spawnPoint.position;
                spawnParticle.Play();
                StartCoroutine(Wait("Respawn", 3f));
                break;

            case "Respawn":
                current.GetCharacter().SetActive(true);
                health = initialHealth;
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
        if (Input.GetKey(KeyCode.RightArrow) && !wallAtRight)
        {
            RunTo("Right");
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtLeft)
        {
            RunTo("Left");
        }
        else if (current.GetBool("Run"))
        {
            StopRunning();
        }  
    }

    private void ConfuseMovementKeyboard(){
        if (Input.GetKey(KeyCode.RightArrow) && !wallAtLeft)
        {
            RunTo("Left");
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !wallAtRight)
        {
            RunTo("Right");
        }
        else if (current.GetBool("Run"))
        {
            StopRunning();
        }
    }

    private void SwimmingKeyboard(){
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
}

