using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject Canvas;
    public TextMeshProUGUI Message;
    public TextMeshProUGUI DisplayGold;
    public TextMeshProUGUI[] Cost;
    public GameObject Spawner;
    public GameObject[] Item;
    public int[] Gold = new int[3];
    public int[] OGGold = new int[3];
    public int[] Sold = new int[3];
    private GameObject Player;
    public bool[] Limited = new bool[3];
    public int[] Inventory = new int[3];
    public float[] increasePrice = new float[3];

    public void Activate()
    {
        FindObjectOfType<AudioManager>().Play("ShopOpen");
        Player = GameObject.FindWithTag("Player");
        Cursor.visible = true;
        Message.SetText("");
        DisplayGold.SetText(Player.GetComponent<RPlayer>().gold + "");
        Canvas.SetActive(true);
        for (int i = 0; i < Item.Length; i++)
        {
            Cost[i].SetText(Gold[i] + " Gold");
            if (Limited[i] && Inventory[i] == 0)
                Cost[i].SetText("Sold Out");
            if (Limited[i] && Inventory[i] > 0)
            {
                Gold[i] = (int)(OGGold[i] + (float)(Sold[i] * ((increasePrice[i] / 100) * OGGold[i])));
            }

        }
        Player.GetComponent<RPlayer>().canDo = false;
        Time.timeScale = 0;
    }
    public void Exit()
    {
        Player.GetComponent<RPlayer>().canDo = true;
        Canvas.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        GetComponent<Dialogue>().alreadyInteracting = false;
    }
    public void Item1()
    {
        if (Limited[0] && Inventory[0] > 0)
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[0])
            {
                FindObjectOfType<AudioManager>().Play("Buy");
                Message.SetText("");
                Player.GetComponent<RPlayer>().gold -= Gold[0];
                Inventory[0]--;
                Sold[0]++;
                Gold[0] = (int)(OGGold[0] + (float)(Sold[0]*((increasePrice[0] / 100)*OGGold[0])));
                Cost[0].SetText(Gold[0] + " Gold");
                Instantiate(Item[0], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
                if (Inventory[0] == 0)
                    Cost[0].SetText("Sold Out");
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        else if (!Limited[0])
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[0])
            {
                FindObjectOfType<AudioManager>().Play("Buy");
                Message.SetText("");
                Player.GetComponent<RPlayer>().gold -= Gold[0];
                Instantiate(Item[0], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
            }
            else
            {
                Message.SetText("Not enough Gold");

            }
        }
        DisplayGold.SetText(Player.GetComponent<RPlayer>().gold + "");
    }
    public void Item2()
    {
        if (Limited[1] && Inventory[1] > 0)
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[1])
            {
                FindObjectOfType<AudioManager>().Play("Buy");
                Message.SetText("");
                Player.GetComponent<RPlayer>().gold -= Gold[1];
                Inventory[1]--;
                Sold[1]++;
                Gold[1] = (int)(OGGold[1] + (float)(Sold[1] * ((increasePrice[1] / 100) * OGGold[1])));
                Cost[1].SetText(Gold[1] + " Gold");
                Instantiate(Item[1], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
                if (Inventory[1] == 0)
                    Cost[1].SetText("Sold Out");
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        else if (!Limited[1])
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[1])
            {
                FindObjectOfType<AudioManager>().Play("Buy");
                Message.SetText("");
                Player.GetComponent<RPlayer>().gold -= Gold[1];
                Instantiate(Item[1], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        DisplayGold.SetText(Player.GetComponent<RPlayer>().gold + "");
    }
    public void Item3()
    {
        if (Limited[2] && Inventory[2] > 0)
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[2])
            {
                FindObjectOfType<AudioManager>().Play("Buy");
                Message.SetText("");
                Player.GetComponent<RPlayer>().gold -= Gold[2];
                Inventory[2]--;
                Sold[2]++;
                Gold[2] = (int)(OGGold[2] + (float)(Sold[2] * ((increasePrice[2] / 100) * OGGold[2])));
                Cost[2].SetText(Gold[2] + " Gold");
                Instantiate(Item[2], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
                if (Inventory[2] == 0)
                    Cost[2].SetText("Sold Out");
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        else if (!Limited[2])
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[2])
            {
                FindObjectOfType<AudioManager>().Play("Buy");
                Message.SetText("");
                Player.GetComponent<RPlayer>().gold -= Gold[2];
                Instantiate(Item[2], new Vector3(Spawner.transform.position.x + Random.Range(-.5f,.5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        DisplayGold.SetText(Player.GetComponent<RPlayer>().gold + "");
    }
}