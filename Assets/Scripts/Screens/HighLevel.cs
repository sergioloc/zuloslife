using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLevel : MonoBehaviour
{
    public GameObject door3;
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
            door3.SetActive(true);
        }
    }
}
