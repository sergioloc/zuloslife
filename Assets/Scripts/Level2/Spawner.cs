using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstacle;
    private Vector2[] patterns;
    public static bool wait = false;
    public Animator cannon1, cannon2, cannon3;

    private void Start(){
        patterns = new Vector2[3];
        patterns[0] = new Vector2(3f, 0f);
        patterns[1] = new Vector2(0f, -3f);
        patterns[2] = new Vector2(3f, -3f);
    }

    void OnEnable(){
        StartCoroutine(Spawn());
    }

    void OnDisable(){
        wait = true;
        StopCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1.5f);
        while (!wait){
            int rand = Random.Range(0, patterns.Length);
            if (rand == 0){
                cannon1.SetTrigger("Shoot");
                cannon2.SetTrigger("Shoot");
            }
            else if (rand == 1){
                cannon2.SetTrigger("Shoot");
                cannon3.SetTrigger("Shoot");
            }
            else if (rand == 2){
                cannon1.SetTrigger("Shoot");
                cannon3.SetTrigger("Shoot");
            }
            Instantiate(obstacle, new Vector2(transform.position.x, patterns[rand].x + 0.17f), Quaternion.identity);
            Instantiate(obstacle, new Vector2(transform.position.x, patterns[rand].y + 0.17f), Quaternion.identity);
            yield return new WaitForSeconds(LevelTwoValues.timeBtwSpawn);
        }
    }
}
