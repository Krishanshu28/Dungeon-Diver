using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public int damage = 3;
    public Collider2D swordColliderLeft;
    public Collider2D swordColliderRight;

    private void Start()
    {
    }
    

    public void AttackRight()
    {
        swordColliderRight.enabled = true;
    }
    public void AttackLeft()
    {
        swordColliderLeft.enabled = true;

        
    }
    public void StopAttack()
    {
        swordColliderLeft.enabled = false;
        swordColliderRight.enabled = false;
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            print("enemy");
            HealthScript enemy = other.GetComponent<HealthScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
