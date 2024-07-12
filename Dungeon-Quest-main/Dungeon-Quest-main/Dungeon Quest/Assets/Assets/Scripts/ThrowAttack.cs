using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAttack : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private float speed = 1.0f;
    public int damage = 3;

    public void Init(Vector2 dir)
    {
        
        rb.velocity = dir * speed;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // Calculate angle
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Apply rotation
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Solid")
            Destroy(gameObject);

        if (other.tag == "Enemy")
        {
            HealthScript enemy = other.GetComponent<HealthScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);

            }
        }
    }
}

