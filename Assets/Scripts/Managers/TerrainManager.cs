using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct Cell
{
    // Space parameters
    public static Vector2 dimensions;
    public Vector3 position;

    // Game of life parameters
    public GOLState GOL_state;
    public List<GameObject> flowers;

    // Genetic Algoritm parameters
    public BiomType biom;
    public float temperature;
    public float humidity_rate;

    // CONSTRUCTORS
    public Cell(Vector3 position, GOLState state, BiomType biom, float temperature, float humidity_rate) {
        this.position = position;
        this.GOL_state = state;
        this.flowers = new List<GameObject>();
        this.biom = biom;
        this.temperature = temperature;
        this.humidity_rate = humidity_rate;

    }

    public Cell(Cell cell) {
        this.position = cell.position;
        this.GOL_state = cell.GOL_state;
        this.flowers = cell.flowers;
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

    [Header("ROCKS")]
    [SerializeField] private List<Material> biom_rocks_colors;

    [SerializeField] private List<GameObject> rock_prefabs;
    public int nb_rocks = 200;
    public Transform rock_parent;

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
        
        SpawnRock();

        
        GOLManager.Instance.init();
    }

    private void SpawnRock()
    {
        for (int i = 0; i < nb_rocks; i++)
        {
            Vector3 random_position = get_random_position();
            BiomType biom_type = EnvironmentManager.Instance.get_biom(random_position);
            
            MeshRenderer rock_renderer = Instantiate(
                rock_prefabs[Random.Range(0, rock_prefabs.Count)],
                random_position,
                Quaternion.Euler(0, Random.Range(0, 360), 0),
                rock_parent
            ).GetComponentInChildren<MeshRenderer>();

            rock_renderer.material = biom_rocks_colors[(int)biom_type];
        }
    }

    public Vector3 get_random_position()
    {
        Vector3 random_position = Vector3.zero;

        random_position.x = terrain.transform.position.x + Random.Range(0f, terrain.terrainData.size.x);
        random_position.z = terrain.transform.position.z + Random.Range(0f, terrain.terrainData.size.z);

        random_position = get_position_on_nav_mesh(random_position);

        return random_position;
    }

    public Vector3 get_random_position_around(Vector3 base_position, float radius)
    {
        Vector3 random_position = Vector3.zero;
        float angle = Random.Range(0f, 2 * Mathf.PI);

        random_position.x = base_position.x + radius * Mathf.Cos(angle);
        random_position.z = base_position.z + radius * Mathf.Sin(angle);

        random_position = get_position_on_nav_mesh(random_position);

        return random_position;
    }

    public Vector3 get_position_on_nav_mesh(Vector3 world_position)
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(new Vector3(world_position.x, 0, world_position.z), out hit, 100.0F, NavMesh.AllAreas);
        
        return hit.position;
    }
}
