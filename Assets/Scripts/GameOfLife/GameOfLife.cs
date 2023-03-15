using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameOfLife : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefab;

    private  List<List<Cell>> future_cells = new List<List<Cell>>();

    void Awake()
    {
        InvokeRepeating("run", 0f, 0.5f);
    }


     void run()
    {     
        ComputeNextGeneration();
        DrawFlowers();
        Clear();
        TerrainManager.Instance.cells = future_cells;
        future_cells=new List<List<Cell>>();
    }

    private void ComputeNextGeneration()
    {
        for(int i=0; i < TerrainManager.Instance.nbDecoupe; i++)
        {
            List<Cell> future_column = new List<Cell>();

            for(int j=0; j< TerrainManager.Instance.nbDecoupe; j++)
            {
                int nb_of_neighbors = CountNeighbors(i, j);

                Cell cell = TerrainManager.Instance.cells[i][j];
                if(cell.isAlive)
                {
                    if(nb_of_neighbors == 2 || nb_of_neighbors == 3)
                    {
                        future_column.Add(cell);
                    }
                    else
                    {
                        cell.isAlive = false;
                        cell.state = CellState.NOTHING;
                        future_column.Add(cell);
                    }
                }
                else
                {
                    if(nb_of_neighbors == 3)
                    {
                        cell.isAlive = true;
                        cell.state = CellState.FLOWER;
                        future_column.Add(cell);
                    }
                    else
                    {
                        future_column.Add(cell);
                    }
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
                int tmp_x=(x+i+TerrainManager.Instance.nbDecoupe)%TerrainManager.Instance.nbDecoupe;
                int tmp_y=(y+j+TerrainManager.Instance.nbDecoupe)%TerrainManager.Instance.nbDecoupe;
                if(TerrainManager.Instance.cells[tmp_x][tmp_y].isAlive)
                {
                    sum++;
                }
            }
        }
        if(TerrainManager.Instance.cells[x][y].isAlive)
        {
            sum--;
        }
        return sum;
    }



   

    private void DrawFlowers()
    {

         for(int i=0; i< TerrainManager.Instance.nbDecoupe; i++)
        {
            for(int j=0; j< TerrainManager.Instance.nbDecoupe; j++)
            {
                Cell previous_cell = TerrainManager.Instance.cells[i][j];
                Cell future_cell = future_cells[i][j];

                if(future_cell.isAlive && future_cell.state != previous_cell.state){


                    for(int flowers_per_cell=0; flowers_per_cell< 3; flowers_per_cell++)
                    {
                        int random = Random.Range(2*future_cell.biome, 2*future_cell.biome+2);
                        float random_position_x = Random.Range(future_cell.position.x - future_cell.size.x/2, future_cell.position.x + future_cell.size.x/2);
                        float random_position_z = Random.Range(future_cell.position.z - future_cell.size.y/2, future_cell.position.z + future_cell.size.y/2);
                        Vector3 random_position = new Vector3(random_position_x, future_cell.position.y, random_position_z);
                        GameObject flower =Instantiate(prefab[random], random_position, Quaternion.identity, transform );

                        future_cell.primitive.Add(flower);
                    }
                    future_cells[i][j] = future_cell;
                }
            }
        }
    }

    private void Clear()
    {
         for(int i=0; i< TerrainManager.Instance.nbDecoupe; i++)
        {
            for(int j=0; j< TerrainManager.Instance.nbDecoupe; j++)
            {
                Cell previous_cell = TerrainManager.Instance.cells[i][j];
                Cell future_cell = future_cells[i][j];
                if(!future_cell.isAlive && future_cell.state != previous_cell.state){
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