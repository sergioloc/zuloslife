using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class LevelOneValues: MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    static public Character[] characters;
    static public bool isPlayerAlive;
    static public int globalLight;

    void Awake(){
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }
}
