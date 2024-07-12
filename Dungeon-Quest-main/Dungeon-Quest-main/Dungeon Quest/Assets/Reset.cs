using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reset : MonoBehaviour,IInteractable
{
    public GameObject dummy;
    private List<GameObject> _Dummy;
    public GameObject[] Spawners;
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
        FindObjectOfType<AudioManager>().Play("TrainingHall");
        for (int i = 0; i<Spawners.Length;i++) 
        {
            Instantiate(dummy, Spawners[i].gameObject.transform.position, Quaternion.identity);
        }

    }
}
