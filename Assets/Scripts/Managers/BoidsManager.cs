using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoidsManager : MonoBehaviour
{

    [SerializeField]
    private List<Boid> boidPrefab;

    [SerializeField]
    private int nb_swarm = 5;

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

        for (int i = 0; i < nb_swarm; i++)
        {
            random_position.Add(GetRandomPosition());
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

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        GameObject boidInstance = Instantiate(prefab, random_position[swarmIndex], Quaternion.identity, _boids_container);

        Boid boid = boidInstance.GetComponent<Boid>();
        boid.SwarmIndex = swarmIndex;
        _boids.Add(boid);
    }


     public Vector3 GetRandomPosition()
    {
        NavMeshHit hit;
        Vector3 random_position = Vector3.zero;

        do
        {
            random_position.x = Random.Range(terrain.transform.position.x, terrain.terrainData.size.x);
            random_position.z = Random.Range(terrain.transform.position.z, terrain.terrainData.size.z);
           

        } while (!NavMesh.SamplePosition(new Vector3(random_position.x, -10.0f, random_position.z), out hit, 500.0f, NavMesh.AllAreas));

        random_position = hit.position;

        return new Vector3(random_position.x, random_position.y, random_position.z);
    }
}
