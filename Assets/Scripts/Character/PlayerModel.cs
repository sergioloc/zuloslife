using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public int id { get; set; }
    public float health { get; set; }

    public PlayerModel()
    {
        id = 1;
        health = 100f;
    }

}
