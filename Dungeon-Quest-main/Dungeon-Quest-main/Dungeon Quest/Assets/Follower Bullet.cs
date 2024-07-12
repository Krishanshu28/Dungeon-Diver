using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBullet : MonoBehaviour
{
    public float lifetime = 5f;
    public float speed = 3f;
    public int rotspeed = 5;
    public int damage;
    Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = targetRotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroyammo", lifetime);
    }
    // Update is called once per frame
    void Update()
    {
        var dir = target.position - transform.position;
        transform.up = Vector3.MoveTowards(transform.up, dir, rotspeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speed * Time.deltaTime);
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
        if (collision.gameObject.CompareTag("Solid"))
        {
            destroyammo();
        }
    }
}