using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellState {NOTHING, FLOWER};

public struct Cell{
    public bool isAlive;
    public Vector3 position;
    public Vector2 size;
    public CellState state;
    public List<GameObject> primitive;
    public int biome;

    public Cell(bool isAlive, Vector3 position, Vector2 size, CellState state, int biome) {
        this.isAlive = isAlive;
        this.position = position;
        this.size = size;
        this.state = state;
        this.primitive = new List<GameObject>();
        this.biome = biome;
    }

    public Cell(Cell cell) {
        this.isAlive = cell.isAlive;
        this.position = cell.position;
        this.size = cell.size;
        this.state = cell.state;
        this.primitive = cell.primitive;
        this.biome = cell.biome;
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

        random_position.x = Random.Range(0, _terrain.terrainData.size.x);
        random_position.z = Random.Range(0, _terrain.terrainData.size.z);
        random_position.y = _terrain.SampleHeight(random_position);

        return random_position;
    }

    void Start(){
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        float minCoordX = float.MaxValue;
        float minCoordZ = float.MaxValue;

        float maxCoordX = float.MinValue;
        float maxCoordZ = float.MinValue;

        //We look for the minimals and maximals coordinate
        Vector3 terrainMin = _terrain.transform.position;
        Vector3 terrainMax = terrainMin + _terrain.terrainData.size;

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
                float hauteur = _terrain.SampleHeight(new Vector3(positionX, 0, positionZ));

                 //Using Random.range with int is exclusive
                int random = Random.Range(0, 100);

                //HERE CALL THE FUNCTION TO GET THE BIOME


                if(random < 20 ){
                    column.Add(new Cell(true, new Vector3(positionX, hauteur, positionZ), new Vector2(cellSizeX, cellSizeZ), CellState.FLOWER, 1));

                }
                else
                {
                    column.Add(new Cell(false, new Vector3(positionX, hauteur, positionZ), new Vector2(cellSizeX, cellSizeZ), CellState.NOTHING, 1));
                }


               
            }
            cells.Add(column);
    
        }
    }
}
