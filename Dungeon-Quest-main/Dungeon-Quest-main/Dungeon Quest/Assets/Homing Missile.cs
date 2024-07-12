using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingMissile : MonoBehaviour
{
    Transform target;
    public float speed = 3f;
    public int shootRange,followRange;
    bool shootavail = true;
    public GameObject Bullet;
    public float delayshoot;
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
    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponent<HealthScript>().UpdateHealth();
        HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        if (health <= 0)
        {
            anim.SetTrigger("WizardDie");
            HealthBar.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            HealthBar.transform.localScale = new Vector3((float)health / Mhealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        }
        if (Vector3.Distance(transform.position, target.position) < shootRange && shootavail)
        {
            anim.SetBool("WizardAttack",true);
            shootavail = false;
            
        }
        else if (Vector3.Distance(transform.position, target.position) < followRange)
        {
            Move();
        }
        else
        {
            anim.SetBool("WizardWalk",false);
        }
    }
    private void Move()
    {
        anim.SetBool("WizardWalk", true);
        var dir = target.position - transform.position;
        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        if (!(Vector3.Distance(transform.position, target.position) < shootRange))
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("WizardFireBall");
        Instantiate(Bullet, transform.position, Quaternion.identity);
        shootavail = true;
        anim.SetBool("WizardAttack",false);
    }
    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("EnemyDie");
        gameObject.transform.parent.gameObject.GetComponent<RoomBehaviour>().Dead();
        Instantiate(Drop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
