using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    // Space parameters
    public static Vector2 dimensions;
    public Vector3 position;

    // Game of life parameters
    public GOLState GOL_state;
    public List<GameObject> primitive;

    // Genetic Algoritm parameters
    public BiomType biom;
    public float temperature;
    public float humidity_rate;

    // CONSTRUCTORS
    public Cell(Vector3 position, GOLState state, BiomType biom, float temperature, float humidity_rate) {
        this.position = position;
        this.GOL_state = state;
        this.primitive = new List<GameObject>();
        this.biom = biom;
        this.temperature = temperature;
        this.humidity_rate = humidity_rate;

    }

    public Cell(Cell cell) {
        this.position = cell.position;
        this.GOL_state = cell.GOL_state;
        this.primitive = cell.primitive;
        this.biom = cell.biom;
        this.temperature = cell.temperature;
        this.humidity_rate = cell.humidity_rate;
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

        grid = new List<List<Cell>>();

        //We define the size of a cell in the grid
        Cell.dimensions = new Vector2((MAX_X - MIN_X) / (float)NB_CELL_X, (MAX_Z - MIN_Z) / (float)NB_CELL_Z);

        for (int x = 0; x < NB_CELL_X; x++)
        {
            grid.Add(new List<Cell>());

            for (int z = 0; z < NB_CELL_Z; z++)
            {
                float cell_x = terrain.transform.position.x + x * Cell.dimensions.x + Cell.dimensions.x * 0.5F;
                float cell_z = terrain.transform.position.z + z * Cell.dimensions.y + Cell.dimensions.y * 0.5F;
                float cell_y = terrain.transform.position.y + TerrainManager.Instance.terrain.SampleHeight(new Vector3(cell_x, 0, cell_z));
                Vector3 cell_position = new Vector3(cell_x, cell_y, cell_z);

                //HERE CALL THE FUNCTION TO GET THE BIOME
                Environment cell_environment = EnvironmentManager.Instance.get_environment(cell_position);

                grid[x].Add(new Cell(
                    cell_position,
                    GOLManager.Instance.get_random_GOL_state(),
                    cell_environment.biom_type,
                    cell_environment.temperature,
                    cell_environment.humidity_rate
                ));
            }
        }

        for (int z = NB_CELL_Z-1; z >= 0; z--)
        {
            string line = "";

            for (int x = 0; x < NB_CELL_Z; x++)
            {
                line += (int)grid[z][x].biom + " ";
            }

            Debug.Log(line);
        }

        

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
