using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obstaclePatterns;
    public float timeBtwSpawn;
    public static bool wait;
    private int phase;

    private void Start(){
        wait = false;
        int rand = Random.Range(0, obstaclePatterns.Length);
        Instantiate(obstaclePatterns[rand], transform.position, Quaternion.identity); 
    }

    private void FixedUpdate()
    {
        phase = LevelTwoValues.phase;

        if (!wait && phase != 7){
            StartCoroutine(Spawn());
            wait = true;
        }
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
        else if (phase == 6){
            timeBtwSpawn = 2f;
        }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeBtwSpawn);
        int rand = Random.Range(0, obstaclePatterns.Length);
        Instantiate(obstaclePatterns[rand], transform.position, Quaternion.identity);
        wait = false;
    }
}
