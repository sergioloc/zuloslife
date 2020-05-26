using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public GameObject building;
    public int timeBtwSpawn = 0;
    public int maxBuildings = 0;
    private bool hasBuildings;
    
    void Start()
    {
        hasBuildings = true;
        StartCoroutine(SpawnBuilding());
    }

    IEnumerator SpawnBuilding()
    {
        while(hasBuildings){
            if (LevelTwoValues.numBuilding < maxBuildings){
                Instantiate(building, transform.position, Quaternion.identity);
            }
            else{
                hasBuildings = false;
            }
            yield return new WaitForSeconds(timeBtwSpawn);
        }
    }

}
