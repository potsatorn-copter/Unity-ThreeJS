using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpableItem : MonoBehaviour
{
    public enum PickUpType
    {
        Blue,
        Pink,
        Yellow,
    }

    public PickUpType type;
}
