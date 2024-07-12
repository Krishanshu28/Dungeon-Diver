using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

   public float Health
    {
        set
        {
            health = value;

            if(health <= 0)
            {
                Defeated();
            }
        }
        get { return health; }
    }

    public float health = 1f;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            var healthComponent = collision.GetComponent<Health>();
            print("Damage");
            healthComponent.TakeDamage(1);
        }
    }
    public void Defeated()
    {
        //Death Animation played
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        //destroying enemy
        Destroy(gameObject);
    }
}
