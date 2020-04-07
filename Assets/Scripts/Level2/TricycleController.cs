using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TricycleController : MonoBehaviour
{
    public float range;
    public float speed;
    public GameObject particle;
    public Image healthBar;
    public Animator camAnim;
    private Animator kekeoAnim;
    public bool shake = true;
    private float maxHeight;
    private float minHeight;
    public GameObject wheelF, wheelB1, wheelB2;
    private bool freeze, mech;
    public GameObject mechParticle;
    private int phase;
    public GameObject buttonAttack;

    void Start()
    {
        kekeoAnim = gameObject.GetComponent<Animator>();
        freeze = mech = false;
        maxHeight = range;
        minHeight = -range;
    }

    void OnEnable(){
        phase = TrycicleLevelValues.phase;
        if (phase == 1){
            StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 3){
            StartCoroutine(EnableAttackButton(15f));
        }
        else if (phase == 5){
            StartCoroutine(EnableAttackButton(8f));
        }
        else if (phase == 7 && !mech){
            SuitUp();
        }
        else if (phase == 10){
            transform.position = new Vector2(1f, 0f);
        }
    }

    void OnDisable(){
        kekeoAnim.SetBool("Attack", false);
    }

    void Update()
    {
        if (TrycicleLevelValues.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        healthBar.fillAmount = TrycicleLevelValues.health / 10;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            MoveDown();
        }

        //Rotate wheels
        wheelF.transform.Rotate(0, 0, -180 * Time.deltaTime);
        wheelB1.transform.Rotate(0, 0, -180 * Time.deltaTime);
        wheelB2.transform.Rotate(0, 0, -180 * Time.deltaTime);

    }

    public void MoveUp()
    {
        if (transform.position.y < maxHeight && !freeze)
        {
            kekeoAnim.SetTrigger("Jump");
            transform.position = new Vector2(transform.position.x, transform.position.y + range);
            if (shake) camAnim.SetTrigger("Shake2");
            if (!mech) Instantiate(particle, transform.position, Quaternion.identity);
            else Instantiate(particle, new Vector2(0f, transform.position.y), Quaternion.identity);
        }
    }

    public void MoveDown()
    {
        if (transform.position.y > minHeight && !freeze)
        {
            kekeoAnim.SetTrigger("Jump");
            transform.position = new Vector2(transform.position.x, transform.position.y - range);
            if (shake) camAnim.SetTrigger("Shake2");
            if (!mech) Instantiate(particle, transform.position, Quaternion.identity);
            else Instantiate(particle, new Vector2(0f, transform.position.y), Quaternion.identity);
        }
    }

    private void SuitUp(){
        transform.position = new Vector2(transform.position.x, 0f);
        freeze = true;
        mech = true;
        kekeoAnim.SetTrigger("Mech");
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        yield return new WaitForSeconds(7f);
        camAnim.SetTrigger("Shake2");
        freeze = false;
        Instantiate(mechParticle, new Vector2(0f, 0f), Quaternion.identity);
    }

    IEnumerator EnableAttackButton(float sec)
    {
        yield return new WaitForSeconds(sec);
        buttonAttack.SetActive(true);
        StartCoroutine(DisableAttackButton());
    }

    public void Attack(){
        kekeoAnim.SetBool("Attack", true);
        buttonAttack.SetActive(false);
    }

    IEnumerator DisableAttackButton()
    {
        yield return new WaitForSeconds(3f);
        buttonAttack.SetActive(false);
    }
}
