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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 mouse = Input.mousePosition;
        // Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        // RaycastHit hit;
        // if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        // {
        //     Debug.Log(find_terrain(hit.point).name);
        // }
    }

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
