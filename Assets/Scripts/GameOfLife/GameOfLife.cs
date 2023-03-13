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

    void Update()
    {
    }


     void run()
    {     
        DrawFlowers();
        ComputeNextGeneration();
        Clear();
    }

    private void ComputeNextGeneration()
    {
        for(int i=0; i < Grid.Instance.nbDecoupe; i++)
        {
            List<Cell> future_column = new List<Cell>();

            for(int j=0; j< Grid.Instance.nbDecoupe; j++)
            {
                int nb_of_neighbors = CountNeighbors(i, j);

                if(Grid.Instance.cells[i][j].isAlive)
                {
                    if(nb_of_neighbors == 2 || nb_of_neighbors == 3)
                    {
                        future_column.Add(new Cell(true, Grid.Instance.cells[i][j].position, Grid.Instance.cells[i][j].size, State.FLOWER));
                    }
                    else
                    {
                        future_column.Add(new Cell(false, Grid.Instance.cells[i][j].position, Grid.Instance.cells[i][j].size, State.NOTHING));
                    }
                }
                else
                {
                    if(nb_of_neighbors == 3)
                    {
                        future_column.Add(new Cell(true, Grid.Instance.cells[i][j].position, Grid.Instance.cells[i][j].size, State.FLOWER));
                    }
                    else
                    {
                        future_column.Add(new Cell(false, Grid.Instance.cells[i][j].position, Grid.Instance.cells[i][j].size, State.NOTHING));
                    }
                }

            }
            future_cells.Add(future_column);
        }
        Grid.Instance.cells = future_cells;
        future_cells=new List<List<Cell>>();
    }

    private int CountNeighbors(int x, int y)
    {
        int sum = 0;
        for(int i=-1; i<2; i++)
        {
            for(int j=-1; j<2; j++)
            {
                int tmp_x=(x+i+Grid.Instance.nbDecoupe)%Grid.Instance.nbDecoupe;
                int tmp_y=(y+j+Grid.Instance.nbDecoupe)%Grid.Instance.nbDecoupe;
                if(Grid.Instance.cells[tmp_x][tmp_y].isAlive)
                {
                    sum++;
                }
            }
        }
        if(Grid.Instance.cells[x][y].isAlive)
        {
            sum--;
        }
        return sum;
    }



   

    private void DrawFlowers()
    {

         for(int i=0; i< Grid.Instance.nbDecoupe; i++)
        {
            for(int j=0; j< Grid.Instance.nbDecoupe; j++)
            {
                if(Grid.Instance.cells[i][j].isAlive && Grid.Instance.cells[i][j].state == State.FLOWER){

                    //==== CAPSULE IS TO BE REPLACED BY A FLOWER/TREE====
                    GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsule.transform.position = new Vector3(Grid.Instance.cells[i][j].position.x, Grid.Instance.cells[i][j].position.y, Grid.Instance.cells[i][j].position.z);
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
            Debug.Log(flowers.Count);
            for(int i=0; i< flowers.Count; i++)
            {
                
                GameObject flower = flowers[0].primitive;
                flowers.RemoveAt(0);
                flower.transform.Destroy();
           
            }
        }
    }
}