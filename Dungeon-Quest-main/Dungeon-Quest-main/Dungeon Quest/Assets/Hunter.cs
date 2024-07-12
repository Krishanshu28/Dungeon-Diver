using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public int speed = 5;
    Transform target;
    public GameObject Bullet;
    public Collider2D swordColliderLeft;
    public Collider2D swordColliderRight;
    public int meleeDamage;
    public float delayshoot;
    public int followRange,shootRange,meleeRange;
    bool shootavail = true;
    private int health;
    Vector3 tar;
    public int Mhealth = 10;
    public GameObject HealthBar;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool move = true;
    public GameObject Drop;
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
        health = gameObject.GetComponent<HealthScript>().UpdateHealth();
        HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        var dir = target.position - transform.position;
        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        if (health <= 0)
        {
            anim.SetTrigger("HunterDie");
            HealthBar.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
        if (Vector3.Distance(transform.position, target.position) < meleeRange)
        {
            anim.SetBool("HunterMelee", true);
        }
        else if (Vector3.Distance(transform.position, target.position) < shootRange && shootavail)
        {
            shootavail = false;
            move = false;
            anim.SetBool("HunterAttack", true);
        }
        else if (Vector3.Distance(transform.position, target.position) < followRange)
        {
            Move();
        }
        else
        {
            anim.SetBool("HunterWalk", false);
        }

    }
    private void Move()
    {
        if (move)
        {
            anim.SetBool("HunterWalk", true);
            if (Vector3.Distance(transform.position, target.position) < shootRange && Vector3.Distance(transform.position, target.position) > meleeRange) { }
            else { transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); }
        }

    }
    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("HunterBow");
        Vector3 Look = transform.InverseTransformPoint(target.position);
        float angle = Mathf.Atan2(Look.y,Look.x)* Mathf.Rad2Deg;
        GameObject bull = Instantiate(Bullet, transform.position, Quaternion.Euler(0,0,angle));
        bull.GetComponent<Bullet>().Init(target.position - transform.position);
        shootavail = true;
        move = true;
        anim.SetBool("HunterAttack", false);
    }
    public void AttackFinish()
    {

        anim.SetBool("HunterMelee", false);
        swordColliderRight.enabled = false;
        swordColliderLeft.enabled = false;
    }
    public void Melee()
    {
        if (!spriteRenderer.flipX)
        {
            swordColliderRight.enabled = true;
        }
        else
        {
            swordColliderLeft.enabled = true;
        }
    }
    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("EnemyDie");
        gameObject.transform.parent.gameObject.GetComponent<RoomBehaviour>().Dead();
        Instantiate(Drop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(meleeDamage);
        }
    }
}
