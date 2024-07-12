using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : MonoBehaviour,IInteractable
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("HealingFountain");
        Player = GameObject.FindWithTag("Player");
        Player.GetComponent<Health>().Increasehealth(500);
        Player.GetComponent<RPlayer>().mana = Player.GetComponent<RPlayer>().Maxmana;
    }
}
