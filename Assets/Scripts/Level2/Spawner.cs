using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obstaclePatterns;
    public float timeBtwSpawn;

    private void Start(){
        int rand = Random.Range(0, obstaclePatterns.Length);
        Instantiate(obstaclePatterns[rand], transform.position, Quaternion.identity);
        StartCoroutine(Spawn());
    }
    private void FixedUpdate()
    {
        if (TrycicleLevelValues.phase == 1){
            timeBtwSpawn = 1.8f;
        }
        else if (TrycicleLevelValues.phase == 2){
            timeBtwSpawn = 1.4f;
        }
        else if (TrycicleLevelValues.phase == 3){
            timeBtwSpawn = 1f;
        }
        else if (TrycicleLevelValues.phase == 4){
            timeBtwSpawn = 0.8f;
        }
        else if (TrycicleLevelValues.phase == 5){
            timeBtwSpawn = 0.6f;
        }
        else if (TrycicleLevelValues.phase == 6){
            timeBtwSpawn = 2f;
        } 
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeBtwSpawn);
        int rand = Random.Range(0, obstaclePatterns.Length);
        Instantiate(obstaclePatterns[rand], transform.position, Quaternion.identity);
        StartCoroutine(Spawn());
    }
}
