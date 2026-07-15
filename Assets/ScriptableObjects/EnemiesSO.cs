using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Enemy
{
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "EnemiesSO", menuName = "Scriptable Objects/EnemiesSO")]
public class EnemiesSO : ScriptableObject
{
    public List<Enemy> spawnableEnemies;
}
