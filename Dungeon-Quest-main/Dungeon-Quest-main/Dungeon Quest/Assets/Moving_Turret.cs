using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Moving_Turret : MonoBehaviour
{
    public int speed = 5;
    Transform target;
    public GameObject Bullet;
    private Vector2 dis;
    public float delayshoot;
    public int range = 10;
    bool shootavail=true;
    Vector3 tar;
    public int Mhealth = 10;
    private int health;
    public GameObject HealthBar;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public GameObject Drop;
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponent<HealthScript>().UpdateHealth();
        HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        if (health <= 0)
        {
            anim.SetTrigger("MushDie");
            HealthBar.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
        if (Vector3.Distance(transform.position, target.position) < range)
        {
            if ((target.position.x == transform.position.x || target.position.y == transform.position.y) && shootavail)
            {
                shootavail = false;
                tar = target.position - transform.position;
                anim.SetBool("MushAttack", true);
            }
            else
            {
                Move();
            }
        }
        else
        {
            anim.SetBool("MushWalk", false);
        }
    }
    private void Move()
    {
        anim.SetBool("MushWalk", true);
        var dir = target.position - transform.position;
        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        dis.x = Mathf.Abs(transform.position.x - target.position.x);
        dis.y = Mathf.Abs(transform.position.y - target.position.y);
        if(dis.x<dis.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.position.y, transform.position.z), speed * Time.deltaTime);
        }
        
    }
    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("MushroomEnergyBall");
        GameObject bull = Instantiate(Bullet, transform.position, Quaternion.identity);
        bull.GetComponent<Bullet>().Init(tar);
        shootavail=true;
        anim.SetBool("MushAttack", false);
    }
    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("EnemyDie");
        gameObject.transform.parent.gameObject.GetComponent<RoomBehaviour>().Dead();
        Instantiate(Drop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
