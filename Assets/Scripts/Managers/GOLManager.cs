using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Transform _flower_container;

    [SerializeField]
    private List<GameObject> prefab;

    private  List<List<Cell>> future_cells = new List<List<Cell>>();

    public void init()
    {
        //We define the size of a cell in the grid
        Vector3 terrain_position = TerrainManager.Instance.terrain.transform.position;
        float cell_width = (TerrainManager.Instance.MAX_X - TerrainManager.Instance.MIN_X) / (float) TerrainManager.Instance.NB_CELL_X;
        float cell_length = (TerrainManager.Instance.MAX_Z - TerrainManager.Instance.MIN_Z) / (float) TerrainManager.Instance.NB_CELL_Z;

        Debug.Log("MAX_X : " + TerrainManager.Instance.MAX_X);
        Debug.Log("MIN_X : " + TerrainManager.Instance.MIN_X);
        Debug.Log("NB_CELL_X : " + TerrainManager.Instance.NB_CELL_X);
        Debug.Log(cell_width + " " + cell_length);

        for (int x = 0; x < TerrainManager.Instance.NB_CELL_Z; x++)
        {
            TerrainManager.Instance.grid.Add(new List<Cell>());

            for (int z = 0; z < TerrainManager.Instance.NB_CELL_X; z++)
            {
                float cell_x = terrain_position.x + x * cell_width + cell_width * 0.5F;
                float cell_z = terrain_position.z + z * cell_length + cell_length * 0.5F;
                float cell_y = terrain_position.y + TerrainManager.Instance.terrain.SampleHeight(new Vector3(cell_x, 0, cell_z));

                //Using Random.range with int is exclusive
                int random = UnityEngine.Random.Range(0, 100);

                //HERE CALL THE FUNCTION TO GET THE BIOME


                if (random < 20)
                {
                    TerrainManager.Instance.grid[x].Add(new Cell(
                        new Vector3(cell_x, cell_y, cell_z),
                        new Vector2(cell_width, cell_length),
                        GOLState.ALIVE,
                        1
                    ));
                }
                else
                {
                    TerrainManager.Instance.grid[x].Add(new Cell(
                        new Vector3(cell_x, cell_y, cell_z),
                        new Vector2(cell_width, cell_length),
                        GOLState.DEAD,
                        1
                    ));
                }
            }
        }

        InvokeRepeating("run", 0f, 0.5f);
    }


    void run()
    {     
        ComputeNextGeneration();
        DrawFlowers();
        Clear();
        TerrainManager.Instance.grid = future_cells;
    }

    private void ComputeNextGeneration()
    {
        future_cells = new List<List<Cell>>();

        for (int i=0; i < TerrainManager.Instance.NB_CELL_Z; i++)
        {
            List<Cell> future_column = new List<Cell>();

            for(int j=0; j< TerrainManager.Instance.NB_CELL_X; j++)
            {
                int nb_of_neighbors = CountNeighbors(i, j);

                Cell cell = TerrainManager.Instance.grid[i][j];

                switch(cell.state)
                {
                    case GOLState.DEAD:
                        if (nb_of_neighbors == 3)
                        {
                            cell.state = GOLState.ALIVE;
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
                            cell.state = GOLState.DEAD;
                            future_column.Add(cell);
                        }
                        break;
                    default:
                        Debug.Log("Invalid GOLState passed : " + cell.state);
                        break;
                }
            }
            future_cells.Add(future_column);
        }
    }

    private int CountNeighbors(int x, int y)
    {
        int sum = 0;

        for(int i=-1; i<2; i++)
        {
            for(int j=-1; j<2; j++)
            {
                int tmp_x=(x+i+TerrainManager.Instance.NB_CELL_X)%TerrainManager.Instance.NB_CELL_X;
                int tmp_y=(y+j+TerrainManager.Instance.NB_CELL_Z) %TerrainManager.Instance.NB_CELL_Z;
                
                if(TerrainManager.Instance.grid[tmp_x][tmp_y].state == GOLState.ALIVE)
                {
                    sum++;
                }
            }
        }

        if(TerrainManager.Instance.grid[x][y].state == GOLState.ALIVE)
        {
            sum--;
        }
        return sum;
    }

    private void DrawFlowers()
    {

         for(int i=0; i < TerrainManager.Instance.NB_CELL_X; i++)
        {
            for(int j=0; j < TerrainManager.Instance.NB_CELL_Z; j++)
            {
                Cell previous_cell = TerrainManager.Instance.grid[i][j];
                Cell future_cell = future_cells[i][j];

                if(future_cell.state == GOLState.ALIVE && future_cell.state != previous_cell.state)
                {
                    for(int flowers_per_cell=0; flowers_per_cell< 3; flowers_per_cell++)
                    {
                        int random = Random.Range(2*future_cell.biome, 2*future_cell.biome+2);
                        Vector3 random_position = Vector3.zero;
                        random_position.x = Random.Range(future_cell.position.x - future_cell.size.x/2, future_cell.position.x + future_cell.size.x/2);
                        random_position.z = Random.Range(future_cell.position.z - future_cell.size.y/2, future_cell.position.z + future_cell.size.y/2);
                        random_position.y = TerrainManager.Instance.terrain.SampleHeight(random_position);

                        GameObject flower = Instantiate(prefab[random], random_position, Quaternion.identity, _flower_container);

                        future_cell.primitive.Add(flower);
                    }
                    future_cells[i][j] = future_cell;
                }
            }
        }
    }

    private void Clear()
    {
         for(int i=0; i< TerrainManager.Instance.NB_CELL_X; i++)
        {
            for(int j=0; j< TerrainManager.Instance.NB_CELL_Z; j++)
            {
                Cell previous_cell = TerrainManager.Instance.grid[i][j];
                Cell future_cell = future_cells[i][j];

                if(future_cell.state == GOLState.DEAD && future_cell.state != previous_cell.state){
                    foreach (GameObject primitive in future_cell.primitive)
                    {
                        primitive.transform.Destroy();
                    }
                    future_cell.primitive = new List<GameObject>();
                    future_cells[i][j] = future_cell;
                }
            }
        }

    }
}