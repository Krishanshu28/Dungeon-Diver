using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{

    public float lifetime = 5f;
    public float speed = 3f;
    public int damage;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector2 dir)
    {
        dir.Normalize();
        rb.velocity = dir * speed;
        transform.Rotate(dir);
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroyammo", lifetime);
    }
    public void destroyammo()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            destroyammo();
        }
        else if (collision.gameObject.CompareTag("Solid"))
        {
            destroyammo();
        }

    }
}
