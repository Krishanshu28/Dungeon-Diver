using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour,IInteractable
{
    Animator animator;
    public static bool Saved = false;
    public GameObject player;
    public Shop[] shops;
    public Dialogue[] NPCs;
    public new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        FindObjectOfType<AudioManager>().Play("Theme");

    }
    void Awake()
    {
        if (Saved || MainMenu.loadVillage)
        {

            Debug.Log("Loaded Village");
            Saving data = SaveSystem.LoadVillageData();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    shops[i].Gold[j] = data.Gold[i, j];
                    shops[i].Sold[j] = data.Sold[i, j];
                    shops[i].Inventory[j] = data.Inventory[i, j];
                }
            }
            for (int i = 0; i < NPCs.Length; i++)
            {
                NPCs[i].firsttime = data.FirstTime[i];
            }
            Saved = false;
        }
        else if (MainMenu.newGame)
        {
            Debug.Log("Loaded new Village");
            MainMenu.loadVillage = false; 
            MainMenu.newGame = false;
            shops[0].Gold[0] = 100;
            shops[0].Gold[1] = 150;
            shops[0].Gold[2] = 50;
            shops[0].Sold[0] = 0;
            shops[0].Sold[1] = 0;
            shops[0].Sold[2] = 0;
            shops[0].Inventory[0] = 5;
            shops[0].Inventory[1] = 5;
            shops[0].Inventory[2] = 10;
            shops[1].Gold[0] = 30;
            shops[1].Gold[1] = 50;
            shops[1].Gold[2] = 175;
            shops[1].Sold[0] = 0;
            shops[1].Sold[1] = 0;
            shops[1].Sold[2] = 0;
            shops[1].Inventory[0] = 0;
            shops[1].Inventory[1] = 0;
            shops[1].Inventory[2] = 5;
            shops[2].Gold[0] = 100;
            shops[2].Gold[1] = 150;
            shops[2].Gold[2] = 200;
            shops[2].Sold[0] = 0;
            shops[2].Sold[1] = 0;
            shops[2].Sold[2] = 0;
            shops[2].Inventory[0] = 10;
            shops[2].Inventory[1] = 5;
            shops[2].Inventory[2] = 2;
            for (int i = 0; i < NPCs.Length; i++)
            {
                NPCs[i].firsttime = true;
            }
            SaveSystem.SaveVillage(this);
        }
    }

    public void SavetheVillage()
    {
        SaveSystem.SaveVillage(this);
    }
    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("Teleport");
        SaveSystem.SaveVillage(this);
        player.GetComponent<RPlayer>().SavePlayer();
        Saved = true;
        player.GetComponent<RPlayer>().LockMovement();
        animator.SetTrigger("Teleport");
    }
    public void TP()
    {
        
        animator.ResetTrigger("Teleport");
        camera.enabled = true;
        player.SetActive(false) ;
       

    }
    public void TPend()
    {
        SceneManager.LoadScene("Diff Player");


    }
    public void Entry()
    {
        FindObjectOfType<AudioManager>().Play("Teleport");
        player.SetActive(true);
        camera.enabled = false;

    }
    public void EntryEnd()
    {
        player.GetComponent<RPlayer>().UnLockMovememt();
    }
}
