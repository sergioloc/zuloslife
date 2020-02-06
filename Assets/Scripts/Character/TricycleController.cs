using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TricycleController : MonoBehaviour
{
    private Vector2 targetPos;
    public float incrementor;
    public float speed;
    private float maxHeight;
    private float minHeight;
    public int health = 3;
    public GameObject particle;
    public Animator camAnim;
    public bool shake = true;

    // Start is called before the first frame update
    void Start()
    {
        maxHeight = incrementor;
        minHeight = -incrementor;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < maxHeight)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            if (shake) camAnim.SetTrigger("Shake2");
            targetPos = new Vector2(transform.position.x, transform.position.y + incrementor);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > minHeight) 
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            if (shake) camAnim.SetTrigger("Shake2");
            targetPos = new Vector2(transform.position.x, transform.position.y - incrementor);
        }
    }
}
