using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct LevelMap
{
    public GameObject[,] tileGrid;
}

public class levelManager: MonoBehaviour
{
    public static LevelMap levelMap;
    [SerializeField] GameObject startingMapTile;
    [SerializeField] GameObject tileGenerator;

    public Object[] eastEntrance;
    public Object[] westEntrance;
    public Object[] northEntrance;
    public Object[] southEntrance;
    public Object[] eastBlocked;
    public Object[] westBlocked;
    public Object[] northBlocked;
    public Object[] southBlocked;

    public int levelWidth;
    public int levelHeight;

    // Start is called before the first frame update
    void Start()
    {
        eastEntrance = Resources.LoadAll("EastEntrance");
        westEntrance = Resources.LoadAll("WestEntrance");
        northEntrance = Resources.LoadAll("NorthEntrance");
        southEntrance = Resources.LoadAll("SouthEntrance");

        eastBlocked = Resources.LoadAll("EastBlocked");
        westBlocked = Resources.LoadAll("WestBlocked");
        northBlocked = Resources.LoadAll("NorthBlocked");
        southBlocked = Resources.LoadAll("SouthBlocked");

        levelMap.tileGrid = new GameObject[levelHeight, levelWidth];
        Vector2Int centre = new Vector2Int(Mathf.CeilToInt(levelWidth / 2f) - 1, Mathf.CeilToInt(levelHeight / 2f) - 1);
        levelMap.tileGrid[centre.y, centre.x] = Instantiate(startingMapTile,tileGenerator.transform);

        tileGenerator[] children = levelMap.tileGrid[centre.y, centre.x].GetComponentsInChildren<tileGenerator>();

        foreach (tileGenerator lG in children)
        {
            switch (lG.direction)
            {
                case Direction.NORTH: { lG.spawnPosInGrid = centre + Vector2Int.up; break; }
                case Direction.EAST: { lG.spawnPosInGrid = centre + Vector2Int.right; break; }
                case Direction.SOUTH: { lG.spawnPosInGrid = centre + Vector2Int.down; break; }
                case Direction.WEST: { lG.spawnPosInGrid = centre + Vector2Int.left; break; }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            for (int i = 0; i < levelWidth; i++) 
            {
                for (int j = 0; j < levelHeight; j++)
                {
                    if (levelMap.tileGrid[j, i] != null) Debug.Log("( " + i + ", " + j + "):" + levelMap.tileGrid[j, i].name);
                    else Debug.Log("(" + i + ", " + j + " ):" + "Empty");
                }
            }
        }
    }
}
