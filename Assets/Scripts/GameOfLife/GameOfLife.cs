using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Flower{
        
        public GameObject primitive;
        public Vector2 indexes;

        public Flower(GameObject primitive, Vector2 indexes) {
            this.primitive = primitive;
            this.indexes = indexes;
        }
    };


public class GameOfLife : MonoBehaviour
{

    private  List<List<Cell>> future_cells = new List<List<Cell>>();
    private List<Flower> flowers= new List<Flower>();

    void Awake()
    {
        InvokeRepeating("run", 0f, 0.5f);
    }


     void run()
    {     
        DrawFlowers();
        ComputeNextGeneration();
        Clear();
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
                        future_column.Add(new Cell(true, cell.position, cell.size, CellState.FLOWER));
                    }
                    else
                    {
                        future_column.Add(new Cell(false, cell.position, cell.size, CellState.NOTHING));
                    }
                }
                else
                {
                    if(nb_of_neighbors == 3)
                    {
                        future_column.Add(new Cell(true, cell.position, cell.size, CellState.FLOWER));
                    }
                    else
                    {
                        future_column.Add(new Cell(false, cell.position, cell.size, CellState.NOTHING));
                    }
                }

            }
            future_cells.Add(future_column);
        }
        TerrainManager.Instance.cells = future_cells;
        future_cells=new List<List<Cell>>();
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
                Cell cell = TerrainManager.Instance.cells[i][j];
                if(cell.isAlive && cell.state == CellState.FLOWER){

                    //==== CAPSULE IS TO BE REPLACED BY A FLOWER/TREE====
                    GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsule.transform.position = new Vector3(cell.position.x, cell.position.y, cell.position.z);
                    capsule.transform.SetParent(transform);
                    Renderer r = capsule.GetComponent<Renderer>();
                    r.material.color = new Color(1,0,0,1);
                    //=============================================

                    flowers.Add(new Flower(capsule, new Vector2(i, j)));
                }
            }
        }
    }

    private void Clear()
    {
        if(flowers.Count != 0)
        {
            for(int i=0; i< flowers.Count; i++)
            {
                
                GameObject flower = flowers[0].primitive;
                flowers.RemoveAt(0);
                flower.transform.Destroy();
           
            }
        }
    }
}