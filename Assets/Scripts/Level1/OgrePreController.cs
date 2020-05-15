using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgrePreController : MonoBehaviour
{
    public GameObject ogre;
    public ParticleSystem smoke;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(smoke, transform.position, transform.rotation);
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
