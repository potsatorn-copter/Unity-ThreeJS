using System;
using System.Collections;
using System.Collections.Generic;
using MyPlatformer;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth: MonoBehaviour 
{
    
    [SerializeField] private int health = 100;
    
    [SerializeField] private int _enemyDeadScore;
    
    private int maxHealth = 100;
    

    public void SetHealth(int maxHealth, int health)
    {
        this.maxHealth = maxHealth;
        this.health = health;
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color =Color.white;
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }
        health -= amount;

        StartCoroutine(VisualIndicator(Color.red));
        
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        ScoreManager.instance.AddScore(_enemyDeadScore);
        SoundManager.instance.Play(SoundManager.SoundName.Hurt);
        
        
    }
    
    
}

