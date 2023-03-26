using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//This class is used to manage the boids
//It is used to keep track of the boids and to spawn them
public class BoidsManager : MonoBehaviour
{

    [SerializeField]
    private List<Boid> boidPrefab;

    [SerializeField]
    private int boids_per_swarm = 200;

    [SerializeField]
    private Transform _boids_container;

   

    private List<Boid> _boids;
    private Terrain terrain;
    private List<Vector3> random_position = new List<Vector3>();

    private void Start()
    {
        terrain = Terrain.activeTerrain;

        _boids = new List<Boid>();

        for (int i = 0; i < EnvironmentManager.Instance._bioms.Count; i++)
        {
            random_position.Add(get_biome_position(i));
            for (int j = 0; j < boids_per_swarm; j++)
            {
                SpawnBoid(boidPrefab[i%boidPrefab.Count].gameObject, i);
            }
        }
    }

    private void Update()
    {
        foreach (Boid boid in _boids)
        {
            boid.SimulateMovement(_boids, Time.deltaTime);
        }
    }

    //Instantiate a boid and add it to the list
    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        GameObject boidInstance = Instantiate(prefab, random_position[swarmIndex], Quaternion.identity, _boids_container);

        Boid boid = boidInstance.GetComponent<Boid>();
        boid.SwarmIndex = swarmIndex;
        _boids.Add(boid);
    }


    //Get the position of a biom object
    public Vector3 get_biome_position(int index)
    {
        NavMeshHit hit;
        Vector2 biom_position = EnvironmentManager.Instance.get_biome_position(EnvironmentManager.Instance._bioms[index]);
        Vector3 position = new Vector3(biom_position.x, 0.0f, biom_position.y);

        NavMesh.SamplePosition(new Vector3(position.x, -10.0f, position.z), out hit, 500.0f, NavMesh.AllAreas);

        position = hit.position;

        return new Vector3(position.x, position.y, position.z);

    }
}
