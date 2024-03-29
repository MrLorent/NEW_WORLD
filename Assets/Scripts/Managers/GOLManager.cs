using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum GOLState
{
    DEAD,
    GROWING,
    ALIVE,
    DYING
};

public class GOLManager : Singleton<GOLManager>
{
    [SerializeField]
    private float _spawn_rate;

    [SerializeField]
    private Transform _flower_container;

    [SerializeField]
    private List<GameObject> prefab;

    private  List<List<Cell>> future_grid = new List<List<Cell>>();

    public void init()
    {
        //Every 10 seconds, a new state of the game of life is computed
        InvokeRepeating("run", 0F, 10F);
    }

    //randomly set the state of a cell to alive or dead, it depends on the spawn rate
    //this is used to initialize the grid
    public GOLState get_random_GOL_state()
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random < _spawn_rate)
        {
            return GOLState.ALIVE;
        }
        else
        {
            return GOLState.DEAD;
        }
    }

    private void run()
    {     
        ComputeNextGeneration();
        DrawFlowers();
        Clear();
        TerrainManager.Instance.grid = future_grid;
    }

    private void ComputeNextGeneration()
    {
        future_grid = new List<List<Cell>>();

        for (int x = 0; x < TerrainManager.Instance.NB_CELL_X; x++)
        {
            List<Cell> future_column = new List<Cell>();

            for(int z = 0; z < TerrainManager.Instance.NB_CELL_Z; z++)
            {
                int nb_of_neighbors = CountNeighbors(x, z);

                Cell cell = TerrainManager.Instance.grid[x][z];

                switch(cell.GOL_state)
                {
                    case GOLState.DEAD:
                        if (nb_of_neighbors == 3)
                        {
                            cell.GOL_state = GOLState.ALIVE;
                            future_column.Add(cell);
                        }
                        else
                        {
                            future_column.Add(cell);
                        }
                        break;

                    case GOLState.ALIVE:
                        if (nb_of_neighbors == 2 || nb_of_neighbors == 3)
                        {
                            future_column.Add(cell);
                        }
                        else
                        {
                            cell.GOL_state = GOLState.DEAD;
                            future_column.Add(cell);
                        }
                        break;
                    default:
                        Debug.Log("Invalid GOLState passed : " + cell.GOL_state);
                        break;
                }
            }
            future_grid.Add(future_column);
        }
    }


    //Count the number of neighbors of a cell, if the cell is on the edge of the grid, it will wrap around
    private int CountNeighbors(int x, int y)
    {
        int sum = 0;

        for(int i=-1; i<2; i++)
        {
            for(int j=-1; j<2; j++)
            {
                int tmp_x=(x+i+TerrainManager.Instance.NB_CELL_X)%TerrainManager.Instance.NB_CELL_X;
                int tmp_y=(y+j+TerrainManager.Instance.NB_CELL_Z) %TerrainManager.Instance.NB_CELL_Z;
                
                if(TerrainManager.Instance.grid[tmp_x][tmp_y].GOL_state == GOLState.ALIVE)
                {
                    sum++;
                }
            }
        }

        if(TerrainManager.Instance.grid[x][y].GOL_state == GOLState.ALIVE)
        {
            sum--;
        }
        return sum;
    }

    //Instantiate flowers on the terrain if the cell is alive
    private void DrawFlowers()
    {

         for(int x = 0; x < TerrainManager.Instance.NB_CELL_X; x++)
        {
            for(int z = 0; z < TerrainManager.Instance.NB_CELL_Z; z++)
            {
                Cell previous_cell = TerrainManager.Instance.grid[x][z];
                Cell future_cell = future_grid[x][z];

                if(future_cell.GOL_state == GOLState.ALIVE && future_cell.GOL_state != previous_cell.GOL_state)
                {
                    for(int flowers_per_cell=0; flowers_per_cell< 3; flowers_per_cell++)
                    {
                        int random = Random.Range(2 * (int)future_cell.biom, 2 * (int)future_cell.biom + 2);
                        Vector3 random_position = Vector3.zero;
                        random_position.x = Random.Range(future_cell.position.x - Cell.dimensions.x * 0.5F, future_cell.position.x + Cell.dimensions.x * 0.5F);
                        random_position.z = Random.Range(future_cell.position.z - Cell.dimensions.y * 0.5F, future_cell.position.z + Cell.dimensions.y * 0.5F);
                        random_position.y = TerrainManager.Instance.terrain.SampleHeight(random_position);

                        NavMeshHit hit;
                        NavMesh.SamplePosition(new Vector3(random_position.x, random_position.y, random_position.z), out hit, 20f, NavMesh.AllAreas);

                        GameObject flower = Instantiate(prefab[random], hit.position, Quaternion.identity, _flower_container);
                        flower.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                        future_cell.flowers.Add(flower);
                    }
                    future_grid[x][z] = future_cell;
                }
            }
        }
    }

    //Destroy flowers if the cell is dead
    private void Clear()
    {
         for(int x = 0; x < TerrainManager.Instance.NB_CELL_X; x++)
        {
            for(int z = 0; z < TerrainManager.Instance.NB_CELL_Z; z++)
            {
                Cell previous_cell = TerrainManager.Instance.grid[x][z];
                Cell future_cell = future_grid[x][z];

                if(future_cell.GOL_state == GOLState.DEAD && future_cell.GOL_state != previous_cell.GOL_state){
                    foreach (GameObject flowers in future_cell.flowers)
                    {
                        flowers.transform.Destroy();
                    }
                    future_cell.flowers = new List<GameObject>();
                    future_grid[x][z] = future_cell;
                }
            }
        }

    }
}