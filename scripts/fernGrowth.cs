using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fernGrowth : MonoBehaviour
{
    public lifeManager lifeMana;

    Vector3 contact;

    public sporeManager mng;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //RaycastHit hit;
        //if (Physics.Raycast(contact, Vector3.forward, out hit)) {

        //    Vector2 cellpos = ConvertCoordtoWorldCell(hit.textureCoord);
        //    lifeMana.DrawLife((int)cellpos.x, (int)cellpos.y, lifeMana.celltype);
        //}

       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("spore"))
        {

            contact = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //contactPoint = ConvertCoordtoWorldCell(contactPoint);

            //lifeMana.DrawLife((int)contactPoint.x, (int)contactPoint.y, lifeMana.celltype);

            Debug.Log("Trigger hit!");
            //Debug.Log(contactPoint);
            RaycastHit hit;
            if (Physics.Raycast(contact, Vector3.forward, out hit))
            {

                contact = ConvertCoordtoWorldCell(hit.textureCoord);
                lifeMana.DrawLife((int)contact.x, (int)contact.y, lifeMana.celltype);
                Debug.Log("drawing!!");
                StartCoroutine(killBoid(other.gameObject.GetComponent<sporeBehavior>()));
            }


            //mng.CreateBoid();
        }
   
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("spore"))
        {
            //Vector3 globalPositionOfContact = collision.contacts[0].point;

            //Debug.Log("Collision hit!");
        }
    }

    Vector2 ConvertCoordtoWorldCell(Vector2 coord)
    {
        Vector2 toreturn = Vector2.zero;
        toreturn.x = Mathf.FloorToInt(coord.x * lifeMana.width);
        toreturn.y = Mathf.FloorToInt(coord.y * lifeMana.height);


        return toreturn;
    }

    IEnumerator killBoid(sporeBehavior obj)
    {
        yield return new WaitForSeconds(0.5f);
        obj.gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (obj.gameObject.transform.childCount != 0)
        {
            Destroy(obj.gameObject.transform.GetChild(0).gameObject);
        }
        yield return new WaitForSeconds(1f);
        obj.gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(3f);
        mng.CreateBoid();

        mng.listOfSpores.Remove(obj);
    }
}
