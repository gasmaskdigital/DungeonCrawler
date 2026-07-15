using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TilesSO", menuName = "Scriptable Objects/TilesSO")]
public class TilesSO : ScriptableObject
{
    public List<levelTile> tiles;
}
