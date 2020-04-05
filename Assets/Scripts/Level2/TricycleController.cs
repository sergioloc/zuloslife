using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TricycleController : MonoBehaviour
{
    public float range;
    public float speed;
    public float health = 10;
    public GameObject particle;
    public Image healthBar;
    public Animator camAnim;
    private Animator kekeoAnim;
    public bool shake = true;
    private float maxHeight;
    private float minHeight;
    public GameObject wheelF, wheelB1, wheelB2;
    private bool freeze, mech;

    void Start()
    {
        freeze = mech = false;
        maxHeight = range;
        minHeight = -range;
        kekeoAnim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        healthBar.fillAmount = health / 10;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            MoveDown();
        }
        else if (TrycicleLevelValues.phase == 7 && !mech){
            SuitUp();
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
    }
}
