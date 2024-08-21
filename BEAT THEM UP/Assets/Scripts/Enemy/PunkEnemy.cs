using System;
using System.Collections;
using System.Collections.Generic;
using MyPlatformer;
using UnityEngine;

public class PunkEnemy : MonoBehaviour
{

    private int damage;
    private float speed;

    [SerializeField] private EnemyData data;
    private GameObject player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemyValues();
    }

    // Update is called once per frame
    void Update()
    {
        Swarm();

    }

    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.CompareTag("Player"))
        {
            if (colider.GetComponent<PlayerHealth>() != null)
            {
                colider.GetComponent<PlayerHealth>().Damage(damage);
            }
        }
    }

    private void SetEnemyValues()
    {
        GetComponent<EnemyHealth>().SetHealth(data.hp, data.hp);
        damage = data.damage;
        speed = data.speed;
    }

    private void Swarm()
    {
        if (player != null)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(5, 5, 1);
            }
            else
            {
                transform.localScale = new Vector3(-5, 5, 1);
            }
        }
    }
}

                    
                        
                            
   
    

