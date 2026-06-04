using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public static LevelGrid levelGrid;
    [SerializeField] GameObject startingMapTile;
    [SerializeField] GameObject levelGenerator;

    public Object[] fourWay;
    public Object[] eastToWest;
    public Object[] westToEast;
    public Object[] northToSouth;
    public Object[] southToNorth;

    public int levelWidth;
    public int levelHieght;

    // Start is called before the first frame update
    void Start()
    {
        fourWay = Resources.LoadAll("4-Way");
        eastToWest = Resources.LoadAll("EastToWest");
        westToEast = Resources.LoadAll("WestToEast");
        northToSouth = Resources.LoadAll("NorthToSouth");
        southToNorth = Resources.LoadAll("SouthToNorth");

        levelGrid.levelGrid = new GameObject[levelHieght, levelWidth];
        Vector2Int centre = new Vector2Int(Mathf.CeilToInt(levelWidth / 2f) - 1, Mathf.CeilToInt(levelHieght / 2f) - 1);
        levelGrid.levelGrid[centre.y, centre.x] = Instantiate(startingMapTile,levelGenerator.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            for (int i = 0; i < levelWidth; i++) 
            {
                for (int j = 0; j < levelHieght; j++)
                {
                    if (levelGrid.levelGrid[j, i] != null) Debug.Log("( " + i + ", " + j + "):" + levelGrid.levelGrid[j, i].name);
                    else Debug.Log("(" + i + ", " + j + " ):" + "Empty");
                }
            }
        }
    }
}
