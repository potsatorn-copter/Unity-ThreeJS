using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Data" , menuName = "ScriptableObject/Player" , order = 2)]

public class PlayerData : ScriptableObject

{
    [SerializeField] public int hp;
    [SerializeField] public float attackCoolDown;
    [SerializeField] public float moveSpeed;

}
