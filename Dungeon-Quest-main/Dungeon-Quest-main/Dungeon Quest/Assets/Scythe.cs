using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Scythe : MonoBehaviour
{

    public float lifetime = 5f;
    public float speed = 3f;
    public int damage;
    public Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector2 dir)
    {
        rb.velocity = speed * dir;
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroyammo", lifetime);
    }
    // Update is called once per frame
    void Update()
    {

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
    public void destroyammo()
    {
        Destroy(gameObject);
    }
}
