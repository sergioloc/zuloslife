using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBox : MonoBehaviour
{
    public GameObject tiles, content, stars;
    public GameObject extra;
    public bool bossScreen;

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
        if (collision.gameObject.CompareTag("Player") && !content.activeSelf)
        {
            tiles.SetActive(true);
            content.SetActive(true);
            if (stars != null)
                stars.SetActive(true);
            if (extra != null)
                extra.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!bossScreen)
            {
                tiles.SetActive(false);
                content.SetActive(false);
                if (stars != null)
                    stars.SetActive(false);
                if (extra != null)
                    extra.SetActive(false);
            }      
        }
    }
}
