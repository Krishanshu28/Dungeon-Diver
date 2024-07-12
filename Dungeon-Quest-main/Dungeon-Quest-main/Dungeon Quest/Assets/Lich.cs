using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Lich : MonoBehaviour
{
    private int health;
    public Collider2D swordColliderLeft;
    public Collider2D swordColliderRight;
    public int meleeDamage;
    private bool free=false,canmove=false,rage=false;
    public int speed = 5,tptime,Tprange,Mrange,Rrange;
    Transform target;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    Vector3 tar;
    public GameObject Scythe,Spawner,Ghost;
    public GameObject[] SpawnPoints;
    private IEnumerator coroutine;
    public Slider HealthBar;
    public GameObject Drop;
    // Start is called before the first frame update
    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        health = (int)HealthBar.maxValue;
    }
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BossSpawn");
        coroutine = Teleporter();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponent<HealthScript>().UpdateHealth();
        if(health<HealthBar.maxValue/2 && !rage)
        {
            rage = true;
            tptime = 3;
            speed *= 2;
            anim.SetBool("LichRage",true);
        }
        else if(health<=0)
        {
            anim.SetTrigger("LichDie");
        }
        HealthBar.value= health;
        var dir = target.position - transform.position;
        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        if (free)
        {
            if (Vector3.Distance(transform.position, target.position) < Tprange)
            {
                free = false;
                anim.SetTrigger("LichTeleport");
            }
            else if(Vector3.Distance(transform.position, target.position) < Mrange)
            {
                free = false;
                anim.SetTrigger("LichMelee");
            }
            else if (Vector3.Distance(transform.position, target.position) < Rrange)
            {
                switch (Random.Range(1, 4))
                {
                    case 1:
                        {
                            //Move
                            free = false;
                            canmove = true;
                            anim.SetTrigger("LichMove");
                            break;
                        }
                    case 2:
                        {
                            //Scythe Attack
                            free = false;
                            anim.SetTrigger("LichScythe");
                            break;
                        }
                    case 3:
                        {
                            //Summon
                            free = false;
                            anim.SetTrigger("LichSummon");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            }
        }
        else if (canmove)
        {

            anim.SetTrigger("LichMove");

            //moving
            if(Vector3.Distance(transform.position,target.position)>2f)
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    public void MakeFree()
    {
        free = true;
        canmove = false;
        anim.ResetTrigger("LichMove");
    }
    public void ScytheShoot()
    {
        FindObjectOfType<AudioManager>().Play("BossScytheThrow");
        anim.ResetTrigger("LichScythe");
        GameObject scythe = Instantiate(Scythe, transform.position, Quaternion.identity);
        tar = target.position;
        scythe.GetComponent<Scythe>().Init(tar - transform.position);
    }
    public void Summon()
    {
        FindObjectOfType<AudioManager>().Play("BossSummon");
        anim.ResetTrigger("LichSummon");
        Instantiate(Ghost, Spawner.transform.position, Quaternion.identity);
    }
    public void Melee()
    {
        FindObjectOfType<AudioManager>().Play("BossScythe");
        anim.ResetTrigger("LichMelee");
        if(!spriteRenderer.flipX)
        {
            swordColliderRight.enabled = true;
        }
        else
        {
            swordColliderLeft.enabled = true;
        }
    }
    public void MeleeEnd()
    {

            swordColliderRight.enabled = false;
            swordColliderLeft.enabled = false;
    }
    public void Teleport()
    {
        int ran = Random.Range(0, SpawnPoints.Length);
        transform.position = SpawnPoints[ran].transform.position;
        anim.ResetTrigger("LichTeleport");
    }
    private IEnumerator Teleporter()
    {
        while (true)
        {
            yield return new WaitForSeconds(tptime);
            free = false;
            anim.SetTrigger("LichTeleport");
        }
    }
    public void Die()
    {
        free = false;
        FindObjectOfType<AudioManager>().Play("BossDie");
        Instantiate(Drop, transform.position, Quaternion.identity);
        GameObject.FindWithTag("Player").GetComponent<RPlayer>().lichDied = true;
        Destroy(gameObject);
    }
    public void TakeDamage(int dam)
    {
        health -= dam;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(meleeDamage);
        }
    }
}
