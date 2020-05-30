using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string scene;

    public void LoadScene(){
        SceneManager.LoadScene(scene);
    }

    public void ClickToLoadScene(){
        GetComponent<Animator>().SetTrigger("Pressed");
        StartCoroutine(DelayLoadScene());
    }

    private IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
