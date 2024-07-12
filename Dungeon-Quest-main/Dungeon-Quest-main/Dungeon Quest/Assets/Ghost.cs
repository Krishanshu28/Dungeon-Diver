using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int speed = 5;
    Transform target;
    private bool move = true;
    private Animator anim;
    public int damage = 2;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            var dir = target.position - transform.position;
            if (dir.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("GhostDie");
            move = false;
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Solid"))
        {
            anim.SetTrigger("GhostDie");
            move = false;
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
