using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float frequency;
    public GameObject destroyParticle;

    void OnEnable()
    {
        if (enemy != null)
            StartCoroutine(Spawn());
    }

    void OnDisable(){
        StopCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true){
            Instantiate(enemy, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(frequency);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponHard"))
        {
            if (destroyParticle != null)
                Instantiate(destroyParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
