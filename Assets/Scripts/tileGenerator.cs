using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction {NORTH,EAST,SOUTH,WEST} // 0,1,2,3 in clockwise order

public class tileGenerator : MonoBehaviour
{
    [SerializeField] levelManager controller;

    [SerializeField] GameObject nextLevelTile;
    [SerializeField] public Direction direction;
    [SerializeField] public Vector2Int spawnPosInGrid;

    [SerializeField] int levelWidth;
    [SerializeField] int levelHeight;
    [SerializeField] float gridSize;

    [SerializeField] public bool hasSpawned;


    private void Awake()
    {
        hasSpawned = false;
        levelManager.tileGenerators.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<levelManager>();
        
        levelWidth = controller.levelWidth;
        levelHeight = controller.levelHeight;
        gridSize = controller.gridSize;

        if (spawnPosInGrid.x >= 0 && spawnPosInGrid.x < levelWidth && spawnPosInGrid.y >= 0 && spawnPosInGrid.y < levelHeight
            && levelManager.levelMap.tileGrid[spawnPosInGrid.y, spawnPosInGrid.x].tile == null) spawnNextTile();

        hasSpawned = true;

        bool levelGenComplete = true;
        foreach (tileGenerator tg in levelManager.tileGenerators)
        {
            // Debug.Log(levelManager.tileGenerators.Count());
            if (!tg.hasSpawned)
            {
                levelGenComplete = false;
                break;
            }
        }
        // Debug.Log(levelGenComplete);
        if (levelGenComplete) controller.GetComponent<levelManager>().finaliseLevelGeneration();

    }

    public void spawnNextTile()
    {
        List<levelTile> validTiles = findValidTiles(direction, spawnPosInGrid.x, spawnPosInGrid.y);
        // Debug.Log(validTiles.Length);
        nextLevelTile = validTiles[UnityEngine.Random.Range(0, validTiles.Count)].tile;

        switch (direction)
        {
            case Direction.NORTH:
                controller.assignTileToLevelGrid(
                    Instantiate(nextLevelTile, gameObject.transform.position + Vector3.forward * gridSize, Quaternion.identity, gameObject.transform.parent.parent),
                        spawnPosInGrid.x, spawnPosInGrid.y);
                break;
            case Direction.EAST:
                controller.assignTileToLevelGrid(
                    Instantiate(nextLevelTile, gameObject.transform.position + Vector3.right * gridSize, Quaternion.identity, gameObject.transform.parent.parent),
                        spawnPosInGrid.x, spawnPosInGrid.y);
                break;
            case Direction.SOUTH:
                controller.assignTileToLevelGrid(
                    Instantiate(nextLevelTile, gameObject.transform.position + Vector3.back * gridSize, Quaternion.identity, gameObject.transform.parent.parent),
                        spawnPosInGrid.x, spawnPosInGrid.y);
                break;
            case Direction.WEST:
                controller.assignTileToLevelGrid(
                    Instantiate(nextLevelTile, gameObject.transform.position + Vector3.left * gridSize, Quaternion.identity, gameObject.transform.parent.parent),
                        spawnPosInGrid.x, spawnPosInGrid.y);
                break;
        }
        updateNextTileSpawnPos();

        
    }

    public void updateNextTileSpawnPos() 
    {
        tileGenerator[] children = levelManager.levelMap.tileGrid[spawnPosInGrid.y, spawnPosInGrid.x].tile.GetComponentsInChildren<tileGenerator>();

        foreach (tileGenerator lG in children) 
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

    public List<levelTile> findValidTiles(Direction dir, int x, int y) 
    {
        List<levelTile> validTiles = new();

        // Select tiles that have an entrance to the current tile
        switch (dir) 
        {
            case Direction.NORTH:
                validTiles.AddRange(controller.southEntrance.tiles);
                break;
            case Direction.EAST:
                validTiles.AddRange(controller.westEntrance.tiles);
                break;
            case Direction.SOUTH:
                validTiles.AddRange(controller.northEntrance.tiles);
                break;
            case Direction.WEST:
                validTiles.AddRange(controller.eastEntrance.tiles); 
                break;
        }

        // Remove tiles if new tile is on the border of the map
        if (x == 0) 
        {
            validTiles = Intersection(validTiles, controller.westBlocked.tiles);        
        }
        else if (x == levelWidth - 1)
        {
            validTiles = Intersection(validTiles, controller.eastBlocked.tiles);        
        }

        if (y == 0)
        {
            validTiles = Intersection(validTiles, controller.southBlocked.tiles);
        }
        else if (y == levelHeight - 1)
        {
            validTiles = Intersection(validTiles, controller.northBlocked.tiles);        
        }


        // Remove tiles if new tile already has walls on its edges and ensure we have matching entrances
        if (y < levelHeight - 1) 
        {
            if (levelManager.levelMap.tileGrid[y + 1, x].tile != null)
            {
                if (listContains(controller.southBlocked.tiles, levelManager.levelMap.tileGrid[y + 1, x]))
                {
                    validTiles = Intersection(validTiles, controller.northBlocked.tiles);
                }
                else if (listContains(controller.southEntrance.tiles,levelManager.levelMap.tileGrid[y + 1, x]))
                {
                    validTiles = Intersection(validTiles, controller.northEntrance.tiles);
                }
                //else Debug.Log(levelManager.levelMap.tileGrid[y + 1, x].name + " not found");
            }
        }
        if (y > 0) 
        {
            if (levelManager.levelMap.tileGrid[y - 1, x].tile != null)
            {
                if (listContains(controller.northBlocked.tiles, levelManager.levelMap.tileGrid[y - 1, x]))
                {
                    validTiles = Intersection(validTiles, controller.southBlocked.tiles);
                }
                else if (listContains(controller.northEntrance.tiles, levelManager.levelMap.tileGrid[y - 1, x]))
                {
                    validTiles = Intersection(validTiles, controller.southEntrance.tiles);
                }
                // else Debug.Log(levelManager.levelMap.tileGrid[y - 1, x].name + " not found");
            }
        }
        if (x < levelWidth - 1)
        {
            if (levelManager.levelMap.tileGrid[y, x + 1].tile != null)
            {
                if (listContains(controller.westBlocked.tiles, levelManager.levelMap.tileGrid[y, x + 1]))
                {
                    validTiles = Intersection(validTiles, controller.eastBlocked.tiles);
                }
                else if (listContains(controller.westEntrance.tiles, levelManager.levelMap.tileGrid[y, x + 1]))
                {
                    validTiles = Intersection(validTiles, controller.eastEntrance.tiles);
                }
                // else Debug.Log(levelManager.levelMap.tileGrid[y, x + 1].name + " not found");
            }
        }
        if (x > 0)
        {
            if (levelManager.levelMap.tileGrid[y, x - 1].tile != null)
            {

                if (listContains(controller.eastBlocked.tiles, levelManager.levelMap.tileGrid[y, x - 1]))
                {
                    validTiles = Intersection(validTiles, controller.westBlocked.tiles);
                }
                else if (listContains(controller.eastEntrance.tiles, levelManager.levelMap.tileGrid[y, x - 1]))
                {
                    validTiles = Intersection(validTiles, controller.westEntrance.tiles);
                }
                // else Debug.Log(levelManager.levelMap.tileGrid[y, x - 1].name + " not found");
            }
        }

        return validTiles;
    }

    public List<T> Intersection<T>(List<T> A, List<T> B) 
    {
        List<T> output = new();

        foreach (T a in A) 
        {
            foreach (T b in B) 
            {
                if (a.Equals(b) && !output.Contains(a))
                {
                    output.Add(a);
                    break;
                }
            }
        }

        return output;
    }

    public bool listContains(List<levelTile> list, levelTile item) 
    {
        foreach (levelTile tile in list) 
        {
            if (tile.name == item.name) return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
