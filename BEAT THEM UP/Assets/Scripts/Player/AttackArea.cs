using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AttackArea : MonoBehaviour
    {
        [SerializeField] private int damage = 3;


        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<EnemyHealth>() != null)
            {
                SoundManager.instance.Play(SoundManager.SoundName.Punch);
                EnemyHealth health = collider.GetComponent<EnemyHealth>();
                health.Damage(damage);
            }
        }
    }
