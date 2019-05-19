using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemy;
    private Vector3 position;
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(-8.81f, -2.24f, 0f);
        rotation = new Quaternion();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            generateEnemy();
        }
    }

    public void generateEnemy()
    {
        Instantiate(enemy, position, rotation);
    }
}
