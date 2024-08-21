using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Create new Item", order = 1)]
public class SO_ITEM : ScriptableObject
{
    public Sprite icon;
    public string id;
    public string itemName;
    public string description;
    public int maxStack;

    [Header("In Game Object")] 
    public GameObject gamePrefab;
    
}
