using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sporeBehavior : MonoBehaviour
{
    Vector3 velocity;
    public float speed;
    Vector3 cohesion, alignment, separation, targetSeeking, obstacleAvoiding;

    public sporeManager myManager;

    public int cohesionMultiplier;
    public int avoidanceMultiplier;

    void Start()
    {
        velocity = new Vector3(Random.value, Random.value, Random.value);
        this.transform.right = velocity.normalized; //forward changable to whichever direction

    }

    void Update()
    {
        Vector3 currentPos = this.transform.position;

        //declare the 3 boid behavior vector
        cohesion = Vector3.zero;
        alignment = this.transform.forward;
        separation = Vector3.zero;
        targetSeeking = Vector3.zero;
        obstacleAvoiding = Vector3.zero;

        foreach (sporeBehavior spore in myManager.listOfSpores)
        {
            if (spore == this) continue;

            Vector3 diffDirec = this.transform.position - spore.transform.position; //vector between this boid and one we're checking
            if (diffDirec.magnitude < myManager.separationDist * cohesionMultiplier)
            {

                cohesion += spore.transform.position; //cohesion is the sum of all the swarm's positions, divided by the count
                alignment += spore.velocity; //alignment is the sum of all the swarm's directions, divided by the count

            }
            if (diffDirec.magnitude > 0 && diffDirec.magnitude < myManager.separationDist)
            {  //other boid is too close!
                separation += (diffDirec.normalized / diffDirec.magnitude) * myManager.separationDist;

            }


        }

        //NEW advoidance
        //foreach (sporeBehavior boid in myManager.listOfSpores)
        //{
        //    if (boid == this) continue;

        //    Vector3 obsDirec = this.transform.position - myManager.obstacle.position;
        //    if (obsDirec.magnitude > 0 && obsDirec.magnitude < myManager.separationDist * avoidanceMultiplier)
        //    {
        //        obstacleAvoiding += (obsDirec.normalized / obsDirec.magnitude) * myManager.separationDist;
        //    }
        //}



        //target seeking
        targetSeeking = myManager.target.position - this.transform.position;

        //avoiding
        //obstacleAvoiding = this.transform.position - myManager.obstacle.position;

        cohesion /= myManager.listOfSpores.Count;
        alignment /= myManager.listOfSpores.Count;
        separation /= myManager.listOfSpores.Count;

        Vector3 newVelocity = Vector3.zero;

        newVelocity += alignment * myManager.weight_alignment;
        newVelocity += cohesion * myManager.weight_cohesion;
        newVelocity += separation * myManager.weight_separation;
        newVelocity += targetSeeking * myManager.weight_seeking;
        newVelocity += obstacleAvoiding * myManager.weight_avoiding;

        velocity = Vector3.Lerp(velocity, newVelocity, Time.deltaTime * 0.1f); //blend velocity
        velocity = Limit(velocity, myManager.maxSpeed);

        this.transform.forward = velocity.normalized;


        //move the boid
        this.transform.position = currentPos + velocity * (Time.deltaTime * speed);
    }

    Vector3 Limit(Vector3 v, float max)
    {
        if (v.magnitude > max)
        { //if the vector's magnitude is too large
            return v.normalized * max; //return it at the maximum, by multiplying the normalized vector (length 1) by the maximum
        }
        else
        {
            return v; //otherwise give it back unchanged
        }
    }


    public float gizmoScale = 2f;

    private void OnDrawGizmos()
    {
        //draw gizmos for us
        //called on editor mode
        //wrap it so it only happen in play mode

        if (Application.isPlaying)
        {
            //current heading
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, velocity * gizmoScale);
            //alignment direction
            Gizmos.color = Color.green;
            Gizmos.DrawRay(this.transform.position, alignment * gizmoScale);
            //cohesion
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(this.transform.position, cohesion * gizmoScale);
            //target seeking
            Gizmos.color = Color.white;
            Gizmos.DrawRay(this.transform.position, targetSeeking * gizmoScale);
            //separation
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(this.transform.position, separation * gizmoScale);
            //avoiding
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(this.transform.position, obstacleAvoiding * gizmoScale);

        }
    }
}
