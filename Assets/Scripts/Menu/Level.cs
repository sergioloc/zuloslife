using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level: MonoBehaviour
{
    public int id;
    public string title;
    private int unlocked = 0;
    public Text textTitle;
    public GameObject unlockedImage, lockedImage;

    void Start()
    {
        unlocked = PlayerPrefs.GetInt("UnlockLevel"+id.ToString());
        if (unlocked == 0){
            unlockedImage.SetActive(false);
            lockedImage.SetActive(true);
        }
        else
            textTitle.text = title;
    }
}
