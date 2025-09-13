using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Data" , menuName = "ScriptableObject/Enemy" , order = 1)]

public class EnemyData : ScriptableObject
{
    [SerializeField] public int hp;
    [SerializeField] public int damage;
    [SerializeField] public float speed;

}
