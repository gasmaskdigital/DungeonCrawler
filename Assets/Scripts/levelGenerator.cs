using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] GameObject floor;
    [SerializeField] public Direction direction;
    [SerializeField] public Vector2Int spawnPosInGrid;

    [SerializeField] float RNG; // A Random Number
    [SerializeField] float spawnRate; //Chances of a new tile being made
    [SerializeField] float fourRate; //1 - the Chances of the new tile being a 4-way intersection

    [SerializeField] int levelWidth;
    [SerializeField] int levelHieght;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        
        levelWidth = controller.levelWidth;
        levelHieght = controller.levelHieght;

        spawnRate = 0.9f;
        fourRate = 0.2f;

        //gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = gameObject.transform.parent.gameObject;

        if (spawnPosInGrid.x >= 0 && spawnPosInGrid.x < levelWidth &&
            spawnPosInGrid.y >= 0 && spawnPosInGrid.y < levelHieght) spawnNextTile();
    }

    public void spawnNextTile()
    {
        RNG = UnityEngine.Random.value;
        //Debug.Log(direction + ": " + RNG);

        if (RNG <= spawnRate && gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] == null)
        {
            floor = controller.fourWay[UnityEngine.Random.Range(0, controller.fourWay.Length)].GameObject(); // 
            switch (direction)
            {
                case Direction.NORTH:
                    if (UnityEngine.Random.value < fourRate) floor = controller.northToSouth[UnityEngine.Random.Range(0, controller.northToSouth.Length)].GameObject();
                    gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(floor, gameObject.transform.position + Vector3.forward * 5, Quaternion.identity, gameObject.transform);
                    break;
                case Direction.EAST:
                    if (UnityEngine.Random.value < fourRate) floor = controller.eastToWest[UnityEngine.Random.Range(0, controller.eastToWest.Length)].GameObject();
                    gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(floor, gameObject.transform.position + Vector3.right * 5, Quaternion.identity, gameObject.transform);
                    break;
                case Direction.SOUTH:
                    if (UnityEngine.Random.value < fourRate) floor = controller.southToNorth[UnityEngine.Random.Range(0, controller.southToNorth.Length)].GameObject();
                    gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(floor, gameObject.transform.position + Vector3.back * 5, Quaternion.identity, gameObject.transform);
                    break;
                case Direction.WEST:
                    if (UnityEngine.Random.value < fourRate) floor = controller.westToEast[UnityEngine.Random.Range(0, controller.westToEast.Length)].GameObject();
                    gameController.levelGrid.levelGrid[spawnPosInGrid.y, spawnPosInGrid.x] = Instantiate(floor, gameObject.transform.position + Vector3.left * 5, Quaternion.identity, gameObject.transform);
                    break;
            }
            updateNextTileSpawnPos();
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
