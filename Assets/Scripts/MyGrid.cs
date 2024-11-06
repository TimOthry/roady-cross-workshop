using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid
{
    // Instantiate basic variables
    private int height;
    private int width;
    private int offset;
    private GameObject prefab;
    
    // Constructor
    public MyGrid(int height, int width, int offset) {
        this.height = height;
        this.width = width;
        this.offset = offset;
        this.StartArea();
        this.GenerateMap();
    }

    public int GetHeight() => height;
    public int GetWidth() => width;
    public int GetOffset() => offset;
    public void SetOffset(int addOffset) => this.offset += addOffset;

    public void StartArea() {
        prefab = Resources.Load<GameObject>($"Prefabs/start-area");
        Object.Instantiate(prefab, new Vector3(12.8f, 0, 0), Quaternion.identity);
    }
    
    // Generatees the map based on the chunks generated
    public void GenerateMap() {
        int[] chunk = GenerateChunk();
        string[] row = {"light-grass", "light-road", "rail", "light-river"};
        for(int z = this.GetOffset(); z < this.GetHeight() + this.GetOffset(); z++) {
            for (int x = 0; x < this.GetWidth(); x++) {
                prefab = Resources.Load<GameObject>($"Prefabs/{row[chunk[z - this.GetOffset()] - 1]}");
                if (chunk[z - this.GetOffset()] == 2) {
                    Object.Instantiate(prefab, new Vector3(x * 1.6f + 0.8f, 0, z * 1.6f + 0.8f), Quaternion.identity);
                    x++;
                } else {
                    Object.Instantiate(prefab, new Vector3(x * 1.6f, 0, z * 1.6f), Quaternion.identity);
                    placeObstacle(chunk[z - this.GetOffset()],x, z);
                }

                if (x == this.GetWidth() - 1 && chunk[z - this.GetOffset()] != 1) {
                    InstantiateSpawners(x, z, chunk);
                }
            }
            if (chunk[z - this.GetOffset()] == 2) {
                z++;
            }
        }
    }

    // Instantiates object spawners on both sides on all rows apart from grass
    private void InstantiateSpawners(int x, int z, int[] chunk) {
        float spawnX = Random.Range(0, 2) == 0 ? x * 1.6f + 5f : -5f;
        float destroyX = spawnX == -5f ? x * 1.6f + 30f : -30f;
        Quaternion spawnRotation;

        if (chunk[z - this.GetOffset()] == 2) {
            prefab = Resources.Load<GameObject>("Prefabs/DestroyObject");
            Object.Instantiate(prefab, new Vector3(destroyX, 0, z * 1.6f + 1.6f), Quaternion.identity);
            prefab = Resources.Load<GameObject>("Prefabs/car-spawner");
            spawnRotation = spawnX == -5f ? Quaternion.Euler(0, 180f, 0) : Quaternion.identity;
            Object.Instantiate(prefab, new Vector3(spawnX, 0, z * 1.6f + 1.6f), spawnRotation);
        } else if (chunk[z - this.GetOffset()] == 3 ) {
            prefab = Resources.Load<GameObject>("Prefabs/train-spawner");
        } else if (chunk[z - this.GetOffset()] == 4 ) {
            prefab = Resources.Load<GameObject>("Prefabs/log-spawner");
        }

        // Instantiates spawner and adjusts rotation
        spawnX = Random.Range(0, 2) == 0 ? x * 1.6f + 5f : -5f;
        spawnRotation = spawnX == -5f ? Quaternion.Euler(0, 180f, 0) : Quaternion.identity;
        Object.Instantiate(prefab, new Vector3(spawnX, 0, z * 1.6f), spawnRotation);

        // Instantiate gameobject which destroys
        destroyX = spawnX == -5f ? x * 1.6f + 30f : -30f;
        GameObject destroyPrefab = Resources.Load<GameObject>("Prefabs/DestroyObject");
        Object.Instantiate(destroyPrefab, new Vector3(destroyX, 0, z * 1.6f), Quaternion.identity);
    }

    // TASK 1: Generates a number string where numbers 1234 represent a terrain
    // 1: light-grasss
    // 2: light-road
    // 3: rail
    // 4: light-river
    // The size of roads are 2x2 and the rest are 1x1 so take that into consideration when generating the chunk
    // There is currently a basic script returning an array with fifteen 1s
    // Hint: Use a double number and System.Random
    private int[] GenerateChunk() {
        int length = GetHeight();
        int[] numberArray = new int[length];

        for (int i = 0; i < length; i++) {
            numberArray[i] = 1;
        }
        return numberArray;
    }

    // Task 2: Place a random obstacle on the grass tiles such as rocks, small trees, med trees, large trees
    // You do not have to check for there to be always a passabable row, if the player gets unlucky it 
    // is what it is.
    // Hint check for if terrain is equal to 1 and use System.Random
    private void placeObstacle(int terrain, int x, int z) {
        
    }
}
