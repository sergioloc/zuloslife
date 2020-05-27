using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level: MonoBehaviour
{
    private int id;
    public string title;
    public string intro;
    public string outro;
    public bool unlocked = false;
    public Button button;

    void Start()
    {
        button.onClick.AddListener(() => { GoToLevel(); });
    }

    private void GoToLevel(){
        if (unlocked)
            SceneManager.LoadScene(intro);
    }
}
