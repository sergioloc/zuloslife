﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level: MonoBehaviour
{
    public int id;
    public string title;
    public string intro;
    public string outro;
    public int unlocked = 0;
    public Button button;
    public GameObject unlockedImage, lockedImage;

    void Start()
    {
        button.onClick.AddListener(() => { GoToLevel(); });
        unlocked = PlayerPrefs.GetInt("UnlockLevel"+id.ToString());
        if (unlocked == 0){
            unlockedImage.SetActive(false);
            lockedImage.SetActive(true);
        }
    }

    private void GoToLevel(){
        if (unlocked == 1 && gameObject.transform.position.x > 200 && gameObject.transform.position.x < 370)
            SceneManager.LoadScene(intro);
    }
}