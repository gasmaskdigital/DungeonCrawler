using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction {NORTH,EAST,SOUTH,WEST} // 0,1,2,3 in clockwise order

public struct LevelGrid 
{
    public GameObject[,] levelGrid;
}


public class levelGenerator : MonoBehaviour
{
    [SerializeField] gameController controller;

    [SerializeField] GameObject nextLevelTile;
    [SerializeField] public Direction direction;
    [SerializeField] public Vector2Int spawnPosInGrid;

    [SerializeField] int levelWidth;
    [SerializeField] int levelHeight;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        
        levelWidth = controller.levelWidth;
        levelHeight = controller.levelHeight;

        if (spawnPosInGrid.x >= 0 && spawnPosInGrid.x < levelWidth && spawnPosInGrid.y >= 0 && spawnPosInGrid.y < levelHeight
            && gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] == null) spawnNextTile();
    }

    public void spawnNextTile()
    {
        GameObject[] validTiles = findValidTiles(direction, spawnPosInGrid.x, spawnPosInGrid.y);
        //Debug.Log(validTiles.Length);
        nextLevelTile = validTiles[UnityEngine.Random.Range(0, validTiles.Length)].GameObject();

        switch (direction)
        {
            case Direction.NORTH:
                gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(nextLevelTile, gameObject.transform.position + Vector3.forward * 5, Quaternion.identity, gameObject.transform);
                break;
            case Direction.EAST:
                gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(nextLevelTile, gameObject.transform.position + Vector3.right * 5, Quaternion.identity, gameObject.transform);
                break;
            case Direction.SOUTH:
                gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(nextLevelTile, gameObject.transform.position + Vector3.back * 5, Quaternion.identity, gameObject.transform);
                break;
            case Direction.WEST:
                gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(nextLevelTile, gameObject.transform.position + Vector3.left * 5, Quaternion.identity, gameObject.transform);
                break;
        }
        updateNextTileSpawnPos();
    }

    public void updateNextTileSpawnPos() 
    {
        levelGenerator[] children = gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x].GetComponentsInChildren<levelGenerator>();

        foreach (levelGenerator lG in children) 
        {
            switch (lG.direction)
            {
                case Direction.NORTH: { lG.spawnPosInGrid = spawnPosInGrid + Vector2Int.up; break; }
                case Direction.EAST: { lG.spawnPosInGrid = spawnPosInGrid + Vector2Int.right; break; }
                case Direction.SOUTH: { lG.spawnPosInGrid = spawnPosInGrid + Vector2Int.down; break; }
                case Direction.WEST: { lG.spawnPosInGrid = spawnPosInGrid + Vector2Int.left; break; }
            }
        }
    }

    public GameObject[] findValidTiles(Direction dir, int x, int y) 
    {
        List<GameObject> validTiles = new();

        switch (dir)
        {
            case Direction.NORTH:
                validTiles.AddRange(controller.southEntrance);
                break;
            case Direction.EAST:
                validTiles.AddRange(controller.westEntrance);
                break;
            case Direction.SOUTH:
                validTiles.AddRange(controller.northEntrance);
                break;
            case Direction.WEST:
                validTiles.AddRange(controller.eastEntrance); 
                break;
        }

        GameObject[] entrances = new GameObject[validTiles.Count()];
        validTiles.CopyTo(entrances);

        if (x == 0) 
        {
            validTiles = Intersection(validTiles.ToArray(), controller.westBlocked);        
        }
        else if (x == levelWidth - 1)
        {
            validTiles = Intersection(validTiles.ToArray(), controller.eastBlocked);        
        }

        if (y == 0)
        {
            validTiles = Intersection(validTiles.ToArray(), controller.southBlocked);
        }
        else if (y == levelHeight - 1)
        {
            validTiles = Intersection(validTiles.ToArray(), controller.northBlocked);        
        }

        return validTiles.ToArray();
    }

    public List<GameObject> Intersection(UnityEngine.Object[] A, UnityEngine.Object[] B) 
    {
        List<GameObject> output = new();

        foreach (GameObject a in A) 
        {
            foreach (GameObject b in B) 
            {
                if (a.name == b.name && !output.Contains(a))
                {
                    output.Add(a);
                    break;
                }
            }
        }

        return output;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
