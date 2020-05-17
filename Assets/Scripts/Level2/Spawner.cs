using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstacle;
    private Vector2[] patterns;
    public float timeBtwSpawn;
    public static bool wait = false;
    private int phase;

    private void Start(){
        patterns = new Vector2[3];
        patterns[0] = new Vector2(3f, 0f);
        patterns[1] = new Vector2(0f, -3f);
        patterns[2] = new Vector2(3f, -3f);
    }

    void OnEnable(){
        phase = LevelTwoValues.phase;
        if (phase == 1){
            timeBtwSpawn = 1.6f;
        }
        else if (phase == 2){
            timeBtwSpawn = 1.4f;
        }
        else if (phase == 3){
            timeBtwSpawn = 1f;
        }
        else if (phase == 4){
            timeBtwSpawn = 0.8f;
        }
        else if (phase == 5){
            timeBtwSpawn = 0.6f;
        }
        else
            timeBtwSpawn = 0.6f;

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
            Instantiate(obstacle, new Vector2(transform.position.x, patterns[rand].x), Quaternion.identity);
            Instantiate(obstacle, new Vector2(transform.position.x, patterns[rand].y), Quaternion.identity);
            yield return new WaitForSeconds(timeBtwSpawn);
        }
    }
}
