using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public int speed = 5;
    public int rotspeed = 5;
    Transform target;
    public int range = 10;
    public int Mhealth = 20;
    private int health;
    public Collider2D swordColliderLeft;
    public Collider2D swordColliderRight;
    private bool move = true;
    public GameObject HealthBar,sprite;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public GameObject Drop;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        anim = sprite.gameObject.GetComponent<Animator>();
        spriteRenderer = sprite.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponent<HealthScript>().UpdateHealth();
       
        if (health <= 0)
        {
            move = false;
            anim.SetTrigger("BatDie");
            HealthBar.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
        if (Vector3.Distance(transform.position, target.position) < range)
        {
            if (Vector2.Distance(transform.position, target.position) < 1f)
            {
                anim.SetBool("BatAttack",true);
            }
            else
            {
                Move();
            }
        }
    }
    private void Move()
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
            transform.up = Vector3.MoveTowards(transform.up, dir, rotspeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speed * Time.deltaTime);
            sprite.transform.position = transform.position;
            sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            HealthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("EnemyDie");
        gameObject.transform.parent.gameObject.GetComponent<RoomBehaviour>().Dead();
        Instantiate(Drop,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
    public void Attack()
    {
        FindObjectOfType<AudioManager>().Play("BatBite");
        if (!spriteRenderer.flipX)
        {
            swordColliderRight.enabled = true;
        }
        else
        {
            swordColliderLeft.enabled = true;
        }
    }
    public void AttackFinish()
    {
        anim.SetBool("BatAttack",false);
        swordColliderRight.enabled = false;
        swordColliderLeft.enabled = false;
    }
}
