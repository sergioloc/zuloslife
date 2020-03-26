using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorCollision : MonoBehaviour
{
    public GameObject contraDoor, floor5, level1;

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
            contraDoor.SetActive(true);
            level1.SetActive(false);
            floor5.SetActive(false);
        }
    }
}
