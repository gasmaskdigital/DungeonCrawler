using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public static LevelGrid levelGrid;
    [SerializeField] GameObject startingMapTile;
    [SerializeField] GameObject levelGenerator;

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

        levelGrid.levelGrid = new GameObject[levelHeight, levelWidth];
        Vector2Int centre = new Vector2Int(Mathf.CeilToInt(levelWidth / 2f) - 1, Mathf.CeilToInt(levelHeight / 2f) - 1);
        levelGrid.levelGrid[centre.y, centre.x] = Instantiate(startingMapTile,levelGenerator.transform);

        levelGenerator[] children = levelGrid.levelGrid[centre.y, centre.x].GetComponentsInChildren<levelGenerator>();

        foreach (levelGenerator lG in children)
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
                    if (levelGrid.levelGrid[j, i] != null) Debug.Log("( " + i + ", " + j + "):" + levelGrid.levelGrid[j, i].name);
                    else Debug.Log("(" + i + ", " + j + " ):" + "Empty");
                }
            }
        }
    }
}
