using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellState {NOTHING, FLOWER, TREE};

public struct Cell{
    public bool isAlive;
    public Vector3 position;
    public Vector2 size;
    public CellState state;

    public Cell(bool isAlive, Vector3 position, Vector2 size, CellState state) {
        this.isAlive = isAlive;
        this.position = position;
        this.size = size;
        this.state = state;
    }
};


public class TerrainManager : Singleton<TerrainManager>
{
    [SerializeField]
    private Terrain _terrain;

    public int nbDecoupe = 100;
    [HideInInspector] public List<List<Cell>> cells = new List<List<Cell>>();


    public Vector3 get_random_position()
    {
        Vector3 random_position = Vector3.zero;

        random_position.x = _terrain.transform.position.x + Random.Range(0, _terrain.terrainData.size.x);
        random_position.z = _terrain.transform.position.z + Random.Range(0, _terrain.terrainData.size.z);
        random_position.y = _terrain.transform.position.y + _terrain.SampleHeight(random_position);

        return random_position;
    }

    void Start(){
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        //We look for the minimals and maximals coordinate
        Vector3 terrainMin = _terrain.transform.position;
        Vector3 terrainMax = terrainMin + _terrain.terrainData.size;

        float minCoordX = terrainMin.x;
        float minCoordZ = terrainMin.z;

        float maxCoordX = terrainMax.x;
        float maxCoordZ = terrainMax.z;

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

                float positionX = _terrain.transform.position.x + i * cellSizeX + halfCellSizeX;
                float positionZ = _terrain.transform.position.z +j * cellSizeZ+halfCellSizeZ;
                float hauteur = _terrain.transform.position.y + _terrain.SampleHeight(new Vector3(positionX, 0, positionZ));

                 //Using Random.range with int is exclusive
                int random = Random.Range(0, 100);

                if(random < 20 ){
                    column.Add(new Cell(true, new Vector3(positionX, hauteur, positionZ), new Vector2(cellSizeX, cellSizeZ), CellState.FLOWER));

                }
                else
                {
                    column.Add(new Cell(false, new Vector3(positionX, hauteur, positionZ), new Vector2(cellSizeX, cellSizeZ), CellState.NOTHING));
                }


               
            }
            cells.Add(column);
    
        }
    }
}
