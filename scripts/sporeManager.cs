using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sporeManager : MonoBehaviour
{
    public GameObject sporePrefab;
    public List<sporeBehavior> listOfSpores = new List<sporeBehavior>(); //list updates

    [Header("Boid stats")]
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

        for (int i = 0; i < numberOfBoids; i++)
        {
            CreateBoid();
        }
    }

    private void Update()
    {

    }

    public void CreateBoid()
    {
        GameObject newSporeObject = Instantiate(sporePrefab, this.transform.position, Quaternion.identity);
        sporeBehavior newSpore = newSporeObject.GetComponent<sporeBehavior>();
        newSpore.myManager = this;
        listOfSpores.Add(newSpore);
        newSpore.transform.parent = this.transform;

        newSpore.speed = maxSpeed * Random.Range(0.8f, 1.2f);
    }

    public void deleteBoid()
    {
       // listOfSpores.Remove();
    }
}
