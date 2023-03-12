using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_finder : MonoBehaviour
{
    public Terrain bot_left;
    public Terrain top_left;
    public Terrain bot_right;
    public Terrain top_right;
    public Terrain parent;

    List<Terrain> terrains;
    

    public Terrain find_terrain(Vector3 coordinate)
    {
        //left side
        if(coordinate.x <= parent.terrainData.size.x/2)
        {
            if(coordinate.z < parent.terrainData.size.z/2)
            {
                return bot_left;
            }

            return top_left;
        }
        //right side
        else
        {
            if(coordinate.z < parent.terrainData.size.z/2)
            {
                return bot_right;
            }

            return top_right;
        }
    }
}
