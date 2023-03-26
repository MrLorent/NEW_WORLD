using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    public float NoClumpingRadius;
    public float LocalAreaRadius;
    public float SteeringSpeed;

    private float speed;

    [HideInInspector]
    public int SwarmIndex;

    private Terrain terrain;


    private void Start() {
        speed = GetComponent<UnityEngine.AI.NavMeshAgent>().speed; 
        terrain = Terrain.activeTerrain;
    }

    //This method is applying the steering forces to the boid and moving it
    //the steering forces are calculated with the cohesion, separation and alignment factors
    public void SimulateMovement(List<Boid> other, float time)
    {
        //default vars
        Vector3 steering = Vector3.zero;


        //separation vars
        Vector3 separationDirection = Vector3.zero;
        int separationCount = 0;

        //alignment vars
        Vector3 alignmentDirection = Vector3.zero;
        int alignmentCount = 0;

        //cohesion vars
        Vector3 cohesionDirection = Vector3.zero;
        int cohesionCount = 0;

        foreach (Boid boid in other)
        {
            //skip self
            if (boid == this)
                continue;

            float distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify local neighbour and avoid clumping
            if (distance < NoClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }

             //identify local neighbour and align
            if (distance < LocalAreaRadius && boid.SwarmIndex == this.SwarmIndex)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

            }

            //identify local neighbour and cohere
            if(distance > LocalAreaRadius && boid.SwarmIndex == this.SwarmIndex)
            {
                cohesionDirection += boid.transform.position - transform.position;
                cohesionCount++;
            }
        }

        //calculate average
        if (separationCount > 0)
                separationDirection /= separationCount;

        //flip and normalize
        separationDirection = -separationDirection.normalized;

        //apply to steering
        steering = separationDirection.normalized * 0.4f;
        steering += alignmentDirection.normalized * 0.24f;
        steering += cohesionDirection.normalized * 0.36f;

        //apply steering
        if (steering != Vector3.zero){
           transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);
        }

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, speed)) * time;
    }
}
