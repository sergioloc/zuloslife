using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgrePreController : MonoBehaviour
{
    public GameObject ogre;
    public ParticleSystem smoke;

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
        if (collision.gameObject.tag == "Player")
        {
            smoke.Play();
            StartCoroutine(InvokeOgre());
        }
    }

    IEnumerator InvokeOgre()
    {
        yield return new WaitForSeconds(5);
        ogre.SetActive(true);
        gameObject.SetActive(false);
    }

}
