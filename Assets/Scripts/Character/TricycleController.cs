using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TricycleController : MonoBehaviour
{
    public float range;
    public float speed;
    public int health = 3;
    public GameObject particle;
    public Animator camAnim;
    private Animator kekeoAnim;
    public bool shake = true;
    private float maxHeight;
    private float minHeight;
    public GameObject wheelF, wheelB1, wheelB2;


    void Start()
    {
        maxHeight = range;
        minHeight = -range;
        kekeoAnim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            //Restart
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

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
        if (transform.position.y < maxHeight)
        {
            kekeoAnim.SetTrigger("Jump");
            transform.position = new Vector2(transform.position.x, transform.position.y + range);
            if (shake) camAnim.SetTrigger("Shake2");
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }

    public void MoveDown()
    {
        if (transform.position.y > minHeight)
        {
            kekeoAnim.SetTrigger("Jump");
            transform.position = new Vector2(transform.position.x, transform.position.y - range);
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - range), speed * Time.deltaTime);
            if (shake) camAnim.SetTrigger("Shake2");
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
