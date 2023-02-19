using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrillage : MonoBehaviour
{
    int nbDecoupe = 50;
    // Start is called before the first frame update
    void Start()
    {
        Terrain[] terrains = GetComponentsInChildren<Terrain>();
        float minCoordX = float.MaxValue;
        float minCoordZ = float.MaxValue;

        float maxCoordX = float.MinValue;
        float maxCoordZ = float.MinValue;

        foreach(Terrain terrain in terrains)
        {
           Vector3 terrainMin = terrain.transform.position;
           Vector3 terrainMax = terrainMin + terrain.terrainData.size;

           minCoordX = Mathf.Min(minCoordX, terrainMin.x);
           minCoordZ = Mathf.Min(minCoordZ, terrainMin.z);

           maxCoordX = Mathf.Max(maxCoordX, terrainMax.x);
           maxCoordZ = Mathf.Max(maxCoordZ, terrainMax.z);
        }

        float caseSizeX= (maxCoordX - minCoordX) / nbDecoupe;
        float caseSizeZ= (maxCoordZ - minCoordZ) / nbDecoupe;

        float halfCaseSizeX = caseSizeX/2f;
        float halfCaseSizeZ = caseSizeZ/2f;

        //Tout ce qui est après ce commentaire peut être déplacé dans un autre script plus adapte pour faire spawn des arbres ou des fleurs
        //==================================================================================================================================

        for(int i=0; i<nbDecoupe; i++)
        {
            float positionX = i*caseSizeX + halfCaseSizeX;
            float positionZ = i*caseSizeZ+halfCaseSizeZ;

            //On recupere le terrain sur lequel est la case pour pouvoir recuperer la hauteur plus tard
            Terrain terrain;
            if(positionX < maxCoordX/2)
            {
                terrain = (positionZ < maxCoordZ/2) ? terrains[0] : terrains[1];
            }
            else
            {
                terrain = (positionZ < maxCoordZ/2) ? terrains[2] : terrains[3];

            }

            float hauteur = terrain.SampleHeight(new Vector3(positionX, 0, positionZ));

            GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.transform.position = new Vector3(positionX, hauteur, positionZ);
        }

        //==================================================================================================================================

    }
}
