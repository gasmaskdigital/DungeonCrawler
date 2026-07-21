using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
public struct levelTile 
{
    public string name;
    public GameObject tile;
    public int xPos;
    public int yPos;
}

public struct LevelMap
{
    public levelTile[,] tileGrid;
    public int tileCount;
    public levelTile staircaseTile;
    public levelTile centreTile;
    public List<levelTile> enemySpawnerTiles;
    public List<levelTile> chestTiles;
}

public class levelManager: MonoBehaviour
{
    [Header("References")]
    public static LevelMap levelMap;
    [SerializeField] GameObject startingMapTile;
    [SerializeField] GameObject startingTileGenerator;
    [SerializeField] public static List<tileGenerator> tileGenerators;
    [SerializeField] GameObject staircase;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject chest;
    [SerializeField] public EnemiesSO allEnemies;

    [Header("Parameters")]
    public int levelWidth;
    public int levelHeight;
    public float gridSize;
    [SerializeField] float spawnerPercentage;
    [SerializeField] float lootPercentage;
    [SerializeField] public static int currentLevel;

    [Header("Tiles")]
    public TilesSO eastEntrance;
    public TilesSO westEntrance;
    public TilesSO northEntrance;
    public TilesSO southEntrance;
    public TilesSO eastBlocked;
    public TilesSO westBlocked;
    public TilesSO northBlocked;
    public TilesSO southBlocked;

    

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(currentLevel);
        if (currentLevel == 0) currentLevel++;

        /*eastEntrance = Resources.LoadAll("Room Tiles/EastEntrance");
        westEntrance = Resources.LoadAll("Room Tiles/WestEntrance");
        northEntrance = Resources.LoadAll("Room Tiles/NorthEntrance");
        southEntrance = Resources.LoadAll("Room Tiles/SouthEntrance");

        eastBlocked = Resources.LoadAll("Room Tiles/EastBlocked");
        westBlocked = Resources.LoadAll("Room Tiles/WestBlocked");
        northBlocked = Resources.LoadAll("Room Tiles/NorthBlocked");
        southBlocked = Resources.LoadAll("Room Tiles/SouthBlocked");*/

        levelHeight = Mathf.Min(30, 5 + (currentLevel - 1) * 2);
        levelWidth = Mathf.Min(30, 5 + (currentLevel - 1) * 2);

        tileGenerators = new();
        levelMap.tileGrid = new levelTile[levelHeight, levelWidth];
        levelMap.enemySpawnerTiles = new();
        spawnerPercentage = Mathf.Min(currentLevel / 20f, 0.4f);
        levelMap.chestTiles = new();
        Vector2Int centre = new Vector2Int(Mathf.CeilToInt(levelWidth / 2f) - 1, Mathf.CeilToInt(levelHeight / 2f) - 1);

        assignTileToLevelGrid(Instantiate(startingMapTile, startingTileGenerator.transform), centre.x, centre.y);
        levelMap.centreTile = levelMap.tileGrid[centre.y, centre.x];

        tileGenerator[] children = levelMap.centreTile.tile.GetComponentsInChildren<tileGenerator>();

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
                    if (levelMap.tileGrid[j, i].tile != null) Debug.Log("( " + i + ", " + j + "):" + levelMap.tileGrid[j, i].tile.name);
                    else Debug.Log("(" + i + ", " + j + " ):" + "Empty");
                }
            }
        }
    }

    public void finaliseLevelGeneration() 
    {
        countValidTiles();
        spawnStaircase();
        startingTileGenerator.GetComponent<NavMeshSurface>().BuildNavMesh();
        int enemySpawnerCount = Mathf.FloorToInt(levelMap.tileCount * spawnerPercentage);
        int chestCount = Mathf.FloorToInt(levelMap.tileCount * lootPercentage);
        for( int i = 0; i < enemySpawnerCount; i++) createEnemySpawner();
        for( int i = 0; i < chestCount; i++) createChest();

        spawnEnemies(enemySpawnerCount);
    }

    void spawnEnemies(int enemySpawnerCount) 
    {

        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("enemySpawner");
        if (enemySpawners.Length > 0)
        {
            int numEnemies = Random.Range(currentLevel * 2, currentLevel * 4 + 1);
            List<Enemy> enemySpawnList = constructEnemySpawnList(numEnemies);
            foreach (Enemy enemy in enemySpawnList)
            {
                int index = Random.Range(0, enemySpawnerCount);
                enemySpawnerScript spawner = enemySpawners[index].GetComponent<enemySpawnerScript>();
                //spawner.enemies = spawner.allEnemies.spawnableEnemies;
                spawner.spawnEnemy(enemy);
            }
        }
    }

    List<Enemy> constructEnemySpawnList(int numEnemies) 
    {
        List<Enemy> enemySpawnList = new List<Enemy>();
        Debug.Log("Spawning " + numEnemies + " Enemies");
        while (enemySpawnList.Count < numEnemies)
        {
            int index = Random.Range(0, allEnemies.spawnableEnemies.Count);
            Enemy nextEnemy = allEnemies.spawnableEnemies[index];
            if (nextEnemy.minFloor <= currentLevel) 
            {
                if (nextEnemy.spawnCap != -1)
                {
                    int enemyTypeCount = 0;
                    foreach (Enemy enemy in enemySpawnList) { if (enemy.name == nextEnemy.name) enemyTypeCount++; }
                    if (enemyTypeCount < nextEnemy.spawnCap) enemySpawnList.Add(nextEnemy);
                }
                else enemySpawnList.Add(nextEnemy);
            }
        }
        return enemySpawnList;
    }

    public void spawnStaircase() 
    {
        //Debug.Log("Finding Staircase posistion...");
        levelMap.staircaseTile = findRandomValidTile();
        Instantiate(staircase, levelMap.staircaseTile.tile.transform);
    }

    public void createEnemySpawner()
    {
        List<levelTile> validTiles = new();

        foreach (levelTile tile in findValidTiles()) if(!levelMap.enemySpawnerTiles.Contains(tile) &&
                tile.tile != levelMap.staircaseTile.tile && tile.tile != levelMap.centreTile.tile) validTiles.Add(tile);

        if (validTiles.Count > 0)
        {
            levelTile levelTile = validTiles[UnityEngine.Random.Range(0, validTiles.Count())];
            levelMap.enemySpawnerTiles.Add(levelTile);
            Instantiate(enemySpawner, levelTile.tile.transform);
        }
    }

    public void createChest()
    {

        List<levelTile> validTiles = new();

        foreach (levelTile tile in findValidTiles()) if (!levelMap.enemySpawnerTiles.Contains(tile) &&
                tile.tile != levelMap.staircaseTile.tile && tile.tile != levelMap.centreTile.tile && !levelMap.chestTiles.Contains(tile)) validTiles.Add(tile);

        if (validTiles.Count > 0)
        {
            levelTile levelTile = validTiles[UnityEngine.Random.Range(0, validTiles.Count())];
            levelMap.chestTiles.Add(levelTile);
            Instantiate(chest, levelTile.tile.transform);
        }
    }

    public void countValidTiles() 
    {
        levelMap.tileCount = findValidTiles().Count();
    }

    public void assignTileToLevelGrid(GameObject tile, int x, int y) 
    {
        levelMap.tileGrid[y, x].tile = tile;
        levelMap.tileGrid[y, x].xPos = x;
        levelMap.tileGrid[y, x].yPos = y;
    }

    public levelTile findRandomValidTile() 
    {
        List<levelTile> validTiles = findValidTiles();
        return validTiles[UnityEngine.Random.Range(0, validTiles.Count)];
    }

    public List<levelTile> findValidTiles()
    {
        List<levelTile> validTiles = new List<levelTile>();
        foreach (levelTile tile in levelMap.tileGrid)
        {
            if (tile.tile != null && tile.tile != levelMap.centreTile.tile) validTiles.Add(tile);
        }
        return validTiles;
    }

    public static void increaseLevel() 
    {
        currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Debug.Log(currentLevel);
    }
}
