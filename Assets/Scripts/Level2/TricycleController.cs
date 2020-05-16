using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TricycleController : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public float range;
    public float speed;
    public GameObject particle;
    public Animator camAnim;
    private Animator kekeoAnim;
    public GameObject wheelF, wheelB1, wheelB2;
    private bool freeze, mech;
    public GameObject mechParticle;
    private int phase;
    public GameObject buttonAttack;

    void Start()
    {
        kekeoAnim = gameObject.GetComponent<Animator>();
        freeze = mech = false;
    }

    void OnEnable(){
        phase = LevelTwoValues.phase;
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
            kekeoAnim.SetTrigger("Ulti");
        }
    }

    void OnDisable(){
        kekeoAnim.SetBool("Attack", false);
    }

    void FixedUpdate()
    {
        if (LevelTwoValues.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        healthSlider.value = LevelTwoValues.health;

        //Rotate wheels
        wheelF.transform.Rotate(0, 0, -180 * Time.deltaTime);
        wheelB1.transform.Rotate(0, 0, -180 * Time.deltaTime);
        wheelB2.transform.Rotate(0, 0, -180 * Time.deltaTime);

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
        yield return new WaitForSeconds(3f);
        buttonAttack.SetActive(false);
    }

    public void Attack(){
        kekeoAnim.SetBool("Attack", true);
        buttonAttack.SetActive(false);
    }
}
