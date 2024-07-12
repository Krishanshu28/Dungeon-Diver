using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Animator anim;

    private int hit=5;
    public int unit=0;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Solid"))
        {
            hit--;
            if(hit <= 0)
            {
                anim.SetTrigger("Die");
            }
            anim.SetTrigger("Hit");
        }
    }

    public void reset()
    {
        anim.ResetTrigger("Hit");
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
