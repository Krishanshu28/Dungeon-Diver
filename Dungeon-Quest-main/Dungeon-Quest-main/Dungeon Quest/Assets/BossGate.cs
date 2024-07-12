using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossGate : MonoBehaviour, IInteractable 
{
    public TextMeshProUGUI Text;
    private bool canEnter = false;
    private GameObject Player;
    public Transform Destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Text.text = "Interact to Enter Boss Room";
            canEnter = true;
            Player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("BossDoor");
            Text.text = "";
            canEnter = false;
        }
    }
    public void Interact()
    {
        if (canEnter)
        {
            Player.transform.position = Destination.position;
        }
    }
}
