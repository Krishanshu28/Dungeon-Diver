using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerChild : MonoBehaviour
{
    public GameObject Controller;
    public int meleeDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        Controller.GetComponent<Follower>().Die();
    }
    public void Attack()
    {
        Controller.GetComponent<Follower>().Attack();

    }
    public void AttackFinish()
    {
        Controller.GetComponent<Follower>().AttackFinish();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(meleeDamage);
        }
    }
}
