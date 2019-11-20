using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneCollision : MonoBehaviour
{
    public string scene;
    public float waitTime;
    public GameObject show;
    public Animator transition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            show.SetActive(true);
            transition.SetTrigger("Start");
            StartCoroutine(NextScene());
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Load Scene: " + scene);
        SceneManager.LoadScene(scene);
    }
}
