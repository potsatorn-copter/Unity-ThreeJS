using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemArea : MonoBehaviour
{
    public PickUpableItem.PickUpType receiveType;
    public GameObject correctUI;

    private void OnTriggerEnter(Collider other)
    {
        PickUpableItem item = other.GetComponent<PickUpableItem>();
        if (item == null)
        {
            return;
        }
        else
        {
            if (item.type == receiveType)
            {
                correctUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickUpableItem item = other.GetComponent<PickUpableItem>();
        if (item == null)
        {
            return;
        }
        else
        {
            if (item.type == receiveType)
            {
                correctUI.SetActive(false);
            }
        }
    }
}
