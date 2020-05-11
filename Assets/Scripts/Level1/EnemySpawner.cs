using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float frequency;
    public GameObject destroyParticle;

    void Start()
    {
        if (enemy != null)
            StartCoroutine(Spawn());
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
