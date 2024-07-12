using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : MonoBehaviour
{
    public int gold;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("GoldDrop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
