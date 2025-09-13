using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndPlace : MonoBehaviour
{
    [Header("PickUp")]
    public Transform pickUpCenter;
    public float radiusPickUp = 0.1f;
    public LayerMask pickUpLayer;

    public Collider[] ObjectsInPickUpArea;
    public Transform itemHolding;

    [Header("Carry")]
    public Transform carryPoint;

    [Header("Drop")]
    public Transform DropPoint;
    
    [Header("Throw")]
    public float throwForce = 20f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (itemHolding == null)
            {
                PickUpItem();
            }
            else
            {
                DropItem();
            }
        }
        if (Input.GetMouseButtonDown(0) && itemHolding != null)
        {
            ThrowItem();
        }
    }

    public void PickUpItem()
    {
        ObjectsInPickUpArea = Physics.OverlapSphere(pickUpCenter.position, radiusPickUp, pickUpLayer);

        if (ObjectsInPickUpArea != null)
        {
            float nearestDistance = 9999.0f;
            foreach (Collider collider in ObjectsInPickUpArea)
            {
                if (Vector3.Distance(collider.transform.position, transform.position) < nearestDistance)
                {
                    nearestDistance = Vector3.Distance(collider.transform.position, transform.position);
                    itemHolding = collider.transform;
                }
            }

            itemHolding.transform.parent = carryPoint;
            itemHolding.transform.localPosition = Vector3.zero;
            itemHolding.transform.localRotation = Quaternion.identity;
            itemHolding.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void DropItem()
    {
        itemHolding.transform.parent = null;
        itemHolding.transform.position = DropPoint.position;
        itemHolding.transform.rotation = transform.rotation;
        itemHolding.GetComponent<Rigidbody>().isKinematic = false;

        itemHolding = null;
    }
    public void ThrowItem()
    {
        Rigidbody rb = itemHolding.GetComponent<Rigidbody>();
        itemHolding.transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        itemHolding = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pickUpCenter.position, radiusPickUp);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(carryPoint.position, new Vector3(1,1,1));
    }
}
