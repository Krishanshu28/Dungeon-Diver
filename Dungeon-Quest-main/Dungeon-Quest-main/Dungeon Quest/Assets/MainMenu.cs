using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool newGame = false,loadVillage = false, newGamePlayer = false;
    public GameObject buttons;
    public GameObject BindingsCanvas;
    public GameObject Player;
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Theme");
    }
    public void ActivateButtons()
    {
        buttons.SetActive(true);
    }
    public void LoadGame()
    {
        Player.GetComponent<RPlayer>().LoadPlayer();
        loadVillage = true;
        SceneManager.LoadScene("Village");
    }
    public void NewGame()
    {
        newGame = true;
        newGamePlayer = true;
        Player.GetComponent<RPlayer>().LoadPlayer();
        SceneManager.LoadScene("Village");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Bindings()
    {
        buttons.SetActive(false);
        BindingsCanvas.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
