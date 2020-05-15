using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public GameObject door;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(LoadLevel());
        }
    }   

    private IEnumerator LoadLevel()
    {
        door.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Tricycle");
    }

}
