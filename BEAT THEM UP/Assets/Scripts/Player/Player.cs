using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour
{
    public Animator animator;
    [SerializeField] private PlayerData data;
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float attackCooldown = 1f;
    private GameObject attackArea = default;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private float nextAttackTime;
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackArea = transform.GetChild(0).gameObject;
        SetPlayerValue();
    }

    void Update()
    {
        // Horizontal Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(moveX * data.moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Attack
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 0.5f / data.attackCoolDown;
            animator.SetBool(Attack1, true);
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool(Attack1, false);
        }

        // Flip Sprite
        if (moveX > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }
        
    }

    void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }

    void SetPlayerValue()
    {
        GetComponent<PlayerHealth>().SetHealth(data.hp , data.hp);
    }
    

}




    
    

