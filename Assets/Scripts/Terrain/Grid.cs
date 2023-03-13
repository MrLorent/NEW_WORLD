using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State {NOTHING, FLOWER, TREE};

 public struct Cell{
        public bool isAlive;
        public Vector3 position;
        public Vector2 size;
        public State state;

        public Cell(bool isAlive, Vector3 position, Vector2 size, State state) {
            this.isAlive = isAlive;
            this.position = position;
            this.size = size;
            this.state = state;
        }
    };

public class Grid : Singleton<Grid>
{

    public int nbDecoupe = 100;
    private Terrain terrain;

   [HideInInspector] public List<List<Cell>> cells = new List<List<Cell>>();

    void Start()
    {
        terrain = GetComponent<Terrain>();
        float minCoordX = float.MaxValue;
        float minCoordZ = float.MaxValue;

        float maxCoordX = float.MinValue;
        float maxCoordZ = float.MinValue;

        //We look for the minimals and maximals coordinate
        Vector3 terrainMin = terrain.transform.position;
        Vector3 terrainMax = terrainMin + terrain.terrainData.size;

        minCoordX = terrainMin.x;
        minCoordZ = terrainMin.z;

        maxCoordX = terrainMax.x;
        maxCoordZ = terrainMax.z;

        //We define the size of a cell in the grid
        float cellSizeX= (maxCoordX - minCoordX) / nbDecoupe;
        float cellSizeZ= (maxCoordZ - minCoordZ) / nbDecoupe;

        float halfCellSizeX = cellSizeX/2f;
        float halfCellSizeZ = cellSizeZ/2f;

        for(int i=0; i<nbDecoupe; i++)
        {
            List<Cell> column = new List<Cell>();
            for(int j=0; j<nbDecoupe; j++)
            {

                float positionX = i*cellSizeX + halfCellSizeX;
                float positionZ = j*cellSizeZ+halfCellSizeZ;

               

                //On recupere le terrain sur lequel est la cell pour pouvoirrecuperer la hauteur plus tard

                float hauteur = terrain.SampleHeight(new Vector3(positionX, 0, positionZ));

                 //Using Random.range with int is exclusive
                int random = Random.Range(0, 100);

                if(random < 20 ){
                    column.Add(new Cell(true, new Vector3(positionX, hauteur, positionZ), new Vector2(cellSizeX, cellSizeZ), State.FLOWER));

                }
                else
                {
                    column.Add(new Cell(false, new Vector3(positionX, hauteur, positionZ), new Vector2(cellSizeX, cellSizeZ), State.NOTHING));
                }


               
            }
            cells.Add(column);
    
        }
    }
}
