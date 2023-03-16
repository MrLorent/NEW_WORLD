using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    // Space parameters
    public Vector3 position;
    public Vector2 size;

    // Game of life parameters
    public GOLState state;
    public List<GameObject> primitive;

    // Genetic Algoritm parameters
    public int biome;

    // CONSTRUCTORS
    public Cell(Vector3 position, Vector2 size, GOLState state, int biome) {
        this.position = position;
        this.size = size;
        this.state = state;
        this.primitive = new List<GameObject>();
        this.biome = biome;
    }

    public Cell(Cell cell) {
        this.position = cell.position;
        this.size = cell.size;
        this.state = cell.state;
        this.primitive = cell.primitive;
        this.biome = cell.biome;
    }
};


public class TerrainManager : Singleton<TerrainManager>
{
    public Terrain terrain;
    public float MIN_X { get; private set; }
    public float MIN_Z { get; private set; }
    public float MAX_X { get; private set; }
    public float MAX_Z { get; private set; }

    public int NB_CELL_X { get; private set; } = 100;
    public int NB_CELL_Z { get; private set; } = 100;

    [HideInInspector] public List<List<Cell>> grid;

    void Start(){
        //We look for the minimals and maximals coordinate
        MIN_X = terrain.transform.position.x + terrain.terrainData.bounds.min.x;
        MIN_Z = terrain.transform.position.z + terrain.terrainData.bounds.min.z;

        MAX_X = terrain.transform.position.x + terrain.terrainData.bounds.max.x;
        MAX_Z = terrain.transform.position.z + terrain.terrainData.bounds.max.z;

        Debug.Log("MAX_X : " + TerrainManager.Instance.MAX_X);

        grid = new List<List<Cell>>();

        GOLManager.Instance.init();
    }

    public Vector3 get_random_position()
    {
        Vector3 random_position = Vector3.zero;

        random_position.x = terrain.transform.position.x + UnityEngine.Random.Range(0, terrain.terrainData.size.x);
        random_position.z = terrain.transform.position.z + UnityEngine.Random.Range(0, terrain.terrainData.size.z);
        random_position.y = terrain.transform.position.y + terrain.SampleHeight(random_position);

        return random_position;
    }
}
