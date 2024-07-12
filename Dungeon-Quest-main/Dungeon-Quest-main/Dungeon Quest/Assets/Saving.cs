

using UnityEngine;
[System.Serializable]
public class Saving
{
    //Player Variables
    public int gold;
    public float moveSpeed;
    public int Maxmana;
    public int healthPotion, manaPotion;
    public float dashCooldown;
    public int throwdamage;
    public int swordAttack;
    public bool LichDead;
    //Player Health Variables
    public int maxHealth = 10;
    public float Armour = 0;
    public float Volume = 0;
    public float Health;
    public int Mana;

    public Saving PlayerData(RPlayer player)
    {
        gold = player.gold;
        moveSpeed = player.moveSpeed;
        Maxmana = player.Maxmana;
        healthPotion = player.healthPotion;
        manaPotion = player.manaPotion;
        dashCooldown = player.dashCooldown;
        throwdamage = player.throwAttack.damage;
        swordAttack = player.swordAttack.damage;
        maxHealth = player.health.maxHealth;
        Armour = player.health.Armour;
        LichDead = player.lichDied;
        Volume = player.volume;
        Health = player.health.currentHealth;
        Mana = player.mana;
        return this;
    }


    //Shop Variables
    public int[,] Gold = new int[3, 3];
    public int[,] Sold = new int[3, 3];
    public int[,] Inventory = new int[3, 3];
    public bool[] FirstTime = new bool[9];
    public Saving VillageData(Teleporter data)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Gold[i,j] = data.shops[i].Gold[j];
                Sold[i,j] = data.shops[i].Sold[j];
                Inventory[i,j] = data.shops[i].Inventory[j];
            }
        }
        for(int i = 0;i < data.NPCs.Length; i++)
        {
            FirstTime[i] = data.NPCs[i].firsttime;
        }
        return this;
    }
}
