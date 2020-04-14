using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(callDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator callDestroy()
    {
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }
}
