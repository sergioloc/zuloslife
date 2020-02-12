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
    public bool shake = true;
    private float maxHeight;
    private float minHeight;


    void Start()
    {
        maxHeight = range;
        minHeight = -range;
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
    }

    public void MoveUp()
    {
        if (transform.position.y < maxHeight)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + range);
            if (shake) camAnim.SetTrigger("Shake2");
            Instantiate(particle, transform.position, Quaternion.identity);
        }   
    }

    public void MoveDown()
    {
        if (transform.position.y > minHeight)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - range);
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - range), speed * Time.deltaTime);
            if (shake) camAnim.SetTrigger("Shake2");
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
