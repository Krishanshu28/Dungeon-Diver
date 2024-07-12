using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemySwordAttack : MonoBehaviour
{
    public int damage = 3;

    Vector2 rightAttackOffset;
    public Collider2D swordColliderRight;
    public Collider2D swordColliderLeft;

    public void AttackRight()
    {
        swordColliderRight.enabled = true;
    }
    public void AttackLeft()
    { 

        swordColliderLeft.enabled = true;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
            
        }
    }
}
