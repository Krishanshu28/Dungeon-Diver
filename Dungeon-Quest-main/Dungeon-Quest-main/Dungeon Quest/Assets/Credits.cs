using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Theme");
    }
    public void OnCreditsEnd()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
