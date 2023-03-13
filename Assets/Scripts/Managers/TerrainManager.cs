using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager>
{
    [SerializeField]
    private Terrain _terrain;


    public Vector3 get_random_position()
    {
        Vector3 random_position = Vector3.zero;

        random_position.x = Random.Range(0, _terrain.terrainData.size.x);
        random_position.z = Random.Range(0, _terrain.terrainData.size.z);
        random_position.y = _terrain.SampleHeight(random_position);

        return random_position;
    }
}
