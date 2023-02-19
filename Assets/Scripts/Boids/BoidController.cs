using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    //if we want multiple swarm
    //public int SwarmIndex;
    public float NoClumpingRadius;
    public float LocalAreaRadius;
    public float Speed;
    public float SteeringSpeed;

    //NEED TO BE OPTIMISED
    // public float separationStrength;
    // public float alignmentStrength;
    // public float cohesionStrength;
    // public float leaderStrength;
    // public float POIStrength;

    public void SimulateMovement(List<BoidController> other, float time)
    {
        //default vars
        var steering = Vector3.zero;

        //separation vars
        Vector3 separationDirection = Vector3.zero;
        int separationCount = 0;

        //alignment vars
        Vector3 alignmentDirection = Vector3.zero;
        int alignmentCount = 0;

        //cohesion vars
        Vector3 cohesionDirection = Vector3.zero;
        int cohesionCount = 0;

        //leader vars
        var leaderBoid = other[0];
        float leaderAngle = 180f;

        //POI vars
        Vector3 targetPoint = new Vector3(1,1,1);

        foreach (BoidController boid in other)
        {
            //skip self
            if (boid == this)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify local neighbour for separation
            if (distance < NoClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }

            //identify local neighbour for alignment and cohesion
            if (distance < LocalAreaRadius)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

                cohesionDirection += boid.transform.position - transform.position;
                cohesionCount++;

                var angle = Vector3.Angle(boid.transform.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = boid;
                    leaderAngle = angle;
                }
            }
        }

        //calculate average
        if (separationCount > 0)
                separationDirection /= separationCount;

        //flip and normalize
        separationDirection = -separationDirection.normalized;

        //flip
        cohesionDirection -= transform.position;

//================STEERING CALCULATION====================================

        //apply to steering
        //NEED TO BE OPTIMISED
        steering = separationDirection * 0.5f;
        steering += alignmentDirection * 0.16f;
        steering += cohesionDirection * 0.34f;

        // //local leader
        // if (leaderBoid != null)
        //     steering += (leaderBoid.transform.position - transform.position).normalized * 0.1f;

        //obstacle avoidance
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, LocalAreaRadius, LayerMask.GetMask("Obstacles")))
            steering = -(hitInfo.point - transform.position).normalized * 500;

        //POI
        //steering += (targetPoint - transform.position).normalized ;

//========================================================================

        //apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, Speed)) * time;
    }
}
