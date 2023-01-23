using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_spawn : MonoBehaviour
{
    public uint nb_tree = 10;

    Terrain_finder terrain_finder;

    public GameObject tree;

    // Start is called before the first frame update
    void Start()
    {
        terrain_finder = this.GetComponent<Terrain_finder>();
        for(uint i=0; i<nb_tree; i++)
        {
            int coord_x = (int) Random.Range(0, terrain_finder.parent.terrainData.size.x);
            int coord_z =  (int) Random.Range(0, terrain_finder.parent.terrainData.size.z);
            Vector3 position = new Vector3(coord_x, 0, coord_z);
            position.y= terrain_finder.find_terrain(position).SampleHeight(position);

            Instantiate(tree, position, Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
