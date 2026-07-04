using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct levelTile 
{
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
    public static LevelMap levelMap;
    [SerializeField] GameObject startingMapTile;
    [SerializeField] GameObject startingTileGenerator;
    [SerializeField] public static List<tileGenerator> tileGenerators;
    [SerializeField] GameObject staircase;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject chest;

    [Header("Parameters")]
    public int levelWidth;
    public int levelHeight;
    public float gridSize;
    [SerializeField] float spawnerPercentage;
    [SerializeField] float lootPercentage;
    [SerializeField] public static int currentLevel;

    [Header("Tiles")]
    public Object[] eastEntrance;
    public Object[] westEntrance;
    public Object[] northEntrance;
    public Object[] southEntrance;
    public Object[] eastBlocked;
    public Object[] westBlocked;
    public Object[] northBlocked;
    public Object[] southBlocked;

    

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(currentLevel);
        if (currentLevel == 0) currentLevel++;

        eastEntrance = Resources.LoadAll("EastEntrance");
        westEntrance = Resources.LoadAll("WestEntrance");
        northEntrance = Resources.LoadAll("NorthEntrance");
        southEntrance = Resources.LoadAll("SouthEntrance");

        eastBlocked = Resources.LoadAll("EastBlocked");
        westBlocked = Resources.LoadAll("WestBlocked");
        northBlocked = Resources.LoadAll("NorthBlocked");
        southBlocked = Resources.LoadAll("SouthBlocked");

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
            levelTile levelTile = validTiles[Random.Range(0, validTiles.Count())];
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
            levelTile levelTile = validTiles[Random.Range(0, validTiles.Count())];
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
        return validTiles[Random.Range(0, validTiles.Count)];
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
