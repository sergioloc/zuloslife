using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public Joystick joystick;
    public float speed = 0;
    public bool facingRight = true;
    public float linearDrag = 4f;

    [Header("Jump")]
    public int jumpForce = 0;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Health")]
    public Slider healthBar;
    public int damageFromEnemy = 10;
    public int initialHealth = 100;
    public float health;
    public GameObject deathCollider;

    [Header("Damage")]
    public GameObject deathEffect;
    public GameObject bloodEffect;
    public GameObject frame;
    public float confuseTime = 0;

    [Header("Particles")]
    public ParticleSystem bloodParticle;
    public ParticleSystem confuseParticle;
    public ParticleSystem healthParticle;
    public ParticleSystem spawnParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem slideParticle;
    public GameObject shieldParticle;
    public GameObject dustParticle;

    [Header("Stamina")]
    public Image staminaPanda;
    public Image staminaKero;
    public Image staminaCinamon;
    public Image staminaKutter;
    public Image staminaTrisky;

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
    private Image fillCurrent;

    public GameObject pandaGameObject, keroGameObject, cinamonGameObject, kutterGameObject, triskyGameObject;

    public static PlayerController instance;
    private Transform spawnPoint;
    private Rigidbody2D rb2d;

    //Values
    private float sensitivity = 0.2f;
    private bool wallAtRight = false, wallAtLeft = false, isInWater = false, isJumping = false, isTakingPhoto = false;
    public bool keyboard = false;
    public Transform shootPoint;


    private Character panda, kero, cinamon, kutter, trisky, current;

    public UnityEvent OnKeroAttack, onKutterAttack;

    void Start()
    {
        instance = this;
        health = initialHealth;
        rb2d = GetComponent<Rigidbody2D>();
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
        if (isGrounded) {
            current.SetBool("isJumping", false);
            rb2d.drag = linearDrag;
        }
        else {
            current.SetBool("isJumping", true);
            rb2d.drag = 0f;
        }
    
    }


    // Movement
    private void Movement()
    {
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
            //Slide effect
            if (direction == "Right" && rb2d.velocity.x < -9 ||
                direction == "Left" && rb2d.velocity.x > 9){
                rb2d.drag = 0f;
                current.SetBool("Slide", true);
                slideParticle.Play();
                StartCoroutine(StopSlide());
            }
            else{
                current.SetBool("Run", true);
                rb2d.drag = linearDrag;
            }

            if (isGrounded)
                dustParticle.SetActive(true);
            if (direction == "Right"){
                rb2d.AddForce(Vector2.right * speed * 50);
                if (!facingRight) Flip();
            }
            else{
                rb2d.AddForce(Vector2.left * speed * 50);
                if (facingRight) Flip();
            }

            if (Mathf.Abs(rb2d.velocity.x) > speed){
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * speed, rb2d.velocity.y);
            }
        }  
    }

    private void StopRunning(){
        dustParticle.SetActive(false);
        current.SetBool("Run", false);
    }

    private IEnumerator StopSlide(){
        yield return new WaitForSeconds(0.15f);
        current.SetBool("Slide", false);
    }

    // Action
    public void startAction()
    {
        if (current.GetStaminaFillAmount() >= 1)
        {
            if (current.CompareNameTo("Kero")){
                OnKeroAttack.Invoke();
            }
            else if (current.CompareNameTo("Kutter")){
                onKutterAttack.Invoke();
            }
            else if (current.CompareNameTo("Trisky") && isGrounded)
            {
                current.SetBool("Action", true);
                StartCoroutine(Health());
            }  
            else if (current.CompareNameTo("Cinamon") && isGrounded)
            {
                current.SetBool("Action", true);
                CameraController.instance.Shake(0.9f);
            }
            else if (current.CompareNameTo("Panda") && !current.GetBool("Run")){
                current.SetBool("Action", true);
                isTakingPhoto = true;
                StartCoroutine(NoTakingPhoto());
            }            
        }
        
    }

    private IEnumerator NoTakingPhoto(){
        yield return new WaitForSeconds(0.9f);
        isTakingPhoto = false;
    }

    private IEnumerator Health(){
        if (health >= (initialHealth - 30))
        {
            health = initialHealth;
        }
        else
        {
            health = health + 30;
        }
        healthParticle.Play();
        shieldParticle.SetActive(true);
        yield return new WaitForSeconds(2f);
        shieldParticle.SetActive(false);
    }

    public void stopAction()
    {
        if (current.CompareNameTo("Panda") || current.CompareNameTo("Trisky") || current.CompareNameTo("Cinamon"))
            current.SetBool("Action", false);
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
            TakeDamage(damageFromEnemy);
        }
        else if (collision.gameObject.tag == "OgreQuake")
        {
            TakeDamage(damageFromEnemy);
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
        else if (collision.gameObject.CompareTag("Confuse"))
        {
            StartCoroutine(Confuse());
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

    IEnumerator Confuse(){
        confuseParticle.Play();
        frame.SetActive(true);
        frame.GetComponent<Image>().color = new Color32(142, 0, 91, 255);
        yield return new WaitForSeconds(confuseTime);
        confuseParticle.Stop();
        frame.SetActive(false);
        frame.GetComponent<Image>().color = new Color32(207, 28, 28, 255);
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
        shootPoint.Rotate(0f, 180f, 0f);
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
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
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

