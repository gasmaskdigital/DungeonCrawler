using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Enemy
{
    public string name;
    public GameObject prefab;
    public int minFloor; // The first floor they will start to appear on
    public int spawnCap; // The most of this enemy found on any given floor. Set -1 if no limit
}

[CreateAssetMenu(fileName = "EnemiesSO", menuName = "Scriptable Objects/EnemiesSO")]
public class EnemiesSO : ScriptableObject
{
    public List<Enemy> spawnableEnemies;
}
