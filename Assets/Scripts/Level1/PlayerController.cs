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
    public int slide = 4;
    public bool facingRight = true;
    public float linearDrag = 4f;

    [Header("Jump")]
    public int jumpForce = 0;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Health")]
    public Slider healthSlider;
    public Image healthColor;
    public int initialHealth = 100;
    public float health;
    public GameObject deathCollider;

    [Header("Underwater")]
    public GameObject waterBar;
    private Slider waterSlider;
    public float InitialOxygen = 150;
    private float oxygen;

    [Header("Damage")]
    public int softDamage = 5;
    public int mediumDamage = 10;
    public int hardDamage = 15;
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

    [Header("Sounds")]
    public AudioSource runAudio;
    public AudioSource jumpAudio;
    public AudioSource landAudio;
    public AudioSource slideAudio;
    public AudioSource confusedAudio;
    public AudioSource deniedAudio;

    private Transform spawnPoint;
    private Rigidbody2D rb2d;
    public static PlayerController instance;

    //Values
    private float sensitivity = 0.2f;
    private bool wallAtRight = false, wallAtLeft = false, isInWater = false, isSwimming = false, isJumping = false, isTakingPhoto = false, isConfuse = false;
    public bool keyboard = false, godMode = false;
    public Transform shootPoint;
    private Character panda, kero, cinamon, kutter, trisky, current;
    public UnityEvent OnKeroAttack, OnKutterAttack, OnPlayerAction;

    void Start()
    {
        instance = this;
        health = initialHealth;
        oxygen = InitialOxygen;
        rb2d = GetComponent<Rigidbody2D>();
        waterSlider = waterBar.GetComponent<Slider>();
        panda = LevelOneValues.characters[0];
        kero = LevelOneValues.characters[1];
        cinamon = LevelOneValues.characters[2];
        kutter = LevelOneValues.characters[3];
        trisky = LevelOneValues.characters[4];
        for (int i = 0; i < 5; i++){
            if (LevelOneValues.characters[i].GetCharacter().activeInHierarchy)
                current = LevelOneValues.characters[i];
        }
        current.SetIconColor(new Color32(0, 80, 255, 255));
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        healthSlider.value = health;
        waterSlider.value = oxygen;

        if (oxygen < 0 && health > 0 && !godMode){
            health -= Time.deltaTime * 10f;
            if (health <= 0)
                Die();
        }
        
        if (health > 0){
            if (!keyboard){
                if (isInWater){
                    Swimming();
                }
                else if (isConfuse){
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
                else if (isConfuse){
                    ConfuseMovementKeyboard();
                }
                else {
                    MovementKeyboard();
                }
                KeyboardAction();
                KeyboardJump();
                KeyboardSwitch();
            }
        }
        else{
            runAudio.Stop();
            confusedAudio.Stop();
            dustParticle.SetActive(false);
        }
        
        if (isInWater){
            current.SetBool("Run", false);
            if (isSwimming)
                current.SetBool("isJumping", true);
            else
                StartCoroutine(StartSwimming());
        }
        else if (isGrounded) {
            current.SetBool("isJumping", false);
            rb2d.drag = linearDrag;
        }
        else {
            current.SetBool("isJumping", true);
            rb2d.drag = 0f;
        }

        //Set God Mode
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            if (godMode)
                healthColor.color = new Color32(255, 255, 0, 255);
            else
                healthColor.color = new Color32(207, 28, 28, 255);
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
            runAudio.Stop();
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
            runAudio.Stop();
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
            if (!jumpAudio.isPlaying)
                jumpAudio.Play();
            rb2d.AddForce(Vector2.up * jumpForce * 100);
            StartCoroutine(EnableJump());
        }
    }

    private IEnumerator EnableJump(){
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
        StopCoroutine(EnableJump());
    }

    IEnumerator StartSwimming(){
        yield return new WaitForSeconds(3f);
        current.SetBool("isJumping", true);
        isSwimming = true;
    }

    private void RunTo(string direction){
        if (!isTakingPhoto){
            if (isGrounded){
                //Audio
                if (!runAudio.isPlaying)
                    runAudio.Play();

                //Particle
                dustParticle.SetActive(true);

                //Slide
                if (direction == "Right" && rb2d.velocity.x < -slide ||
                    direction == "Left" && rb2d.velocity.x > slide){
                    rb2d.drag = 0f;
                    current.SetBool("Slide", true);
                    slideParticle.Play();
                    if (!slideAudio.isPlaying)
                        slideAudio.Play();
                    StartCoroutine(StopSlide());
                }
                else{
                    current.SetBool("Run", true);
                    rb2d.drag = linearDrag;
                }
            }
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
            OnPlayerAction.Invoke();
            if (current.CompareNameTo("Kero")){
                OnKeroAttack.Invoke();
            }
            else if (current.CompareNameTo("Kutter")){
                OnKutterAttack.Invoke();
                current.SetBool("Action", true);
            }
            else if (current.CompareNameTo("Trisky"))
            {
                current.SetBool("Action", true);
                StartCoroutine(Heal());
            }  
            else if (current.CompareNameTo("Cinamon") && isGrounded)
            {
                current.SetBool("Action", true);
                CinemachineController.instance.Shake(0.9f);
            }
            else if (current.CompareNameTo("Panda") && !current.GetBool("Run")){
                current.SetBool("Action", true);
                isTakingPhoto = true;
                StartCoroutine(NoTakingPhoto());
            }            
        }
        else if (!deniedAudio.isPlaying){
            deniedAudio.Play();
        }
        
    }

    private IEnumerator NoTakingPhoto(){
        yield return new WaitForSeconds(0.9f);
        isTakingPhoto = false;
    }

    private IEnumerator Heal(){
        health = initialHealth;
        healthParticle.Play();
        shieldParticle.SetActive(true);
        yield return new WaitForSeconds(2f);
        shieldParticle.SetActive(false);
    }

    public void stopAction()
    {
        if (current.CompareNameTo("Panda") || 
            current.CompareNameTo("Trisky") || 
            current.CompareNameTo("Cinamon") ||
            current.CompareNameTo("Kutter"))
            current.SetBool("Action", false);
    }


    // Collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponSoft"))
        {
            TakeDamage(softDamage);
        }
        else if (collision.gameObject.CompareTag("WeaponMedium"))
        {
            TakeDamage(mediumDamage);
        }
        else if (collision.gameObject.CompareTag("WeaponHard"))
        {
            TakeDamage(hardDamage);
        }
        else if (collision.gameObject.CompareTag("Confuse"))
        {
            StartCoroutine(Confuse());
        }
        else if (collision.gameObject.CompareTag("WallLeft"))
        {
            wallAtLeft = true;
        }
        else if (collision.gameObject.CompareTag("WallRight"))
        {
            wallAtRight = true;
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            spawnPoint = collision.gameObject.transform;
        }
        else if (collision.gameObject.CompareTag("BossScreen"))
        {
            CinemachineController.instance.ModifyZoom(10f);
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            rb2d.gravityScale = -0.004f;
            isInWater = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water") && !godMode)
        {
            waterBar.SetActive(true);
            if (oxygen > 0)
                oxygen -= Time.deltaTime * 5f;
        }
        if (collision.gameObject.CompareTag("Air"))
        {
            if (oxygen < 100)
                oxygen += Time.deltaTime * 50f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isGrounded && health > 0 && !landAudio.isPlaying)
        {
            landAudio.Play();
        }
    }

    


    // Aux functions
    private void TakeDamage(float damage)
    {
        if (!godMode){
            frame.SetActive(true);
            if (health > 0)
            {
                health = health - damage;
                bloodParticle.Play();
                if (!current.IsImpactFaceActive())
                {
                    current.SetImpactFaceActive(true);
                    StartCoroutine(HideDamage());
                }
                if (health <= 0)
                    Die();
            }
        }
    }

    IEnumerator HideDamage()
    {
        yield return new WaitForSeconds(1.5f);
        frame.SetActive(false);
        current.SetImpactFaceActive(false);
    }

    IEnumerator Confuse(){
        isConfuse = true;
        confuseParticle.Play();
        confusedAudio.Play();
        frame.SetActive(true);
        frame.GetComponent<Image>().color = new Color32(142, 0, 91, 255);
        yield return new WaitForSeconds(confuseTime);

        isConfuse = false;
        confuseParticle.Stop();
        confusedAudio.Stop();
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
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        deathCollider.SetActive(false);
        waterBar.SetActive(false);
        gameObject.GetComponent<Transform>().position = spawnPoint.position;
        spawnParticle.Play();
        if (!facingRight) Flip();
        yield return new WaitForSeconds(3f);

        current.GetCharacter().SetActive(true);
        health = initialHealth;
        oxygen = InitialOxygen;
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
            runAudio.Stop();
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
            runAudio.Stop();
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

    private void KeyboardSwitch(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            switchTo(trisky);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            switchTo(kutter);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            switchTo(cinamon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)){
            switchTo(kero);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)){
            switchTo(panda);
        }
    }
}

