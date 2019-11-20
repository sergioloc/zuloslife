using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneUI : MonoBehaviour
{
    public string scene;
    public float waitTime;
    public Animator transition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        transition.SetTrigger("Start");
        GetComponent<Animator>().SetTrigger("Pressed");
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Load Scene: " + scene);
        SceneManager.LoadScene(scene);
    }
}
