using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPulse : MonoBehaviour
{
    public GameObject doDamage;
    public ParticleSystem spark;

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
        if (collision.gameObject.tag == "OgreFist")
        {
            spark.Play();
            doDamage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OgreFist")
        {
            doDamage.SetActive(false);
        }
    }

}
