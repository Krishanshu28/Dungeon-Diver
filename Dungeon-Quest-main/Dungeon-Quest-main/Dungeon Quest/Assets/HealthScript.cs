using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int UpdateHealth()
    {
        return health;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

    }
}
