using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour
{
    Vector3 velocity;
    public float speed;
    Vector3 cohesion, alignment, separation, targetSeeking, obstacleAvoiding;

    public int cohesionMultiplier;
    public int avoidanceMultiplier;

    public int numberOfBoids;
    public float maxSpeed, separationDist;

    [Range(0, 2f)] public float weight_alignment;
    [Range(0, 2f)] public float weight_cohesion;
    [Range(0, 1f)] public float weight_separation;
    [Range(0, 2f)] public float weight_seeking;
    [Range(0, 1f)] public float weight_avoiding;

    public Transform target;
    //public Transform obstacle;

    void Start()
    {
        velocity = Vector3.one;
        this.transform.forward = velocity.normalized; //forward changable to whichever direction

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


        //NEW advoidance

            //Vector3 obsDirec = this.transform.position - obstacle.position;
            //if (obsDirec.magnitude > 0 && obsDirec.magnitude < separationDist * avoidanceMultiplier)
            //{
            //    obstacleAvoiding += (obsDirec.normalized / obsDirec.magnitude) * separationDist;
            //}
     



        //target seeking
        targetSeeking = target.position - this.transform.position;

        //avoiding
        //obstacleAvoiding = this.transform.position - myManager.obstacle.position;

        Vector3 newVelocity = Vector3.zero;

        newVelocity += alignment * weight_alignment;
        newVelocity += cohesion * weight_cohesion;
        newVelocity += separation * weight_separation;
        newVelocity += targetSeeking * weight_seeking;
        newVelocity += obstacleAvoiding * weight_avoiding;

        velocity = Vector3.Lerp(velocity, newVelocity, Time.deltaTime * 0.1f); //blend velocity
        velocity = Limit(velocity, maxSpeed);

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
