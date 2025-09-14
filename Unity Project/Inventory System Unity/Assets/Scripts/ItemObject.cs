using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ItemObject : MonoBehaviour
{
    public SO_ITEM item;
    public int amount = 1;
    public TextMeshProUGUI amountText;

    public void SetAmount(int newAmount)
    {
        amount = newAmount;
        amountText.text = amount.ToString();
    }

    public void RandomAmout()
    {
        amount = Random.Range(1, item.maxStack + 1);
        amountText.text = amount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemPicker picker = other.GetComponent<ItemPicker>();
        if (picker != null) // Ensure we have the picker component
        {         
            picker.inventory.AddItem(item, amount);
            Destroy(gameObject);
        }
    }
}
