 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public AttributeToChange attributeToChange = new AttributeToChange();
    public int amountToChangeAttribute;
    
    public bool UseItem()
    {
        if(statToChange == StatToChange.health)
        {
            Health health = GameObject.Find("Player").GetComponent<Health>();
            if(health.currentHealth == health.maxHealth)
            {
                return false;
            }
            else
            {
                health.Increasehealth(amountToChangeStat);
                return true;
            }     
        }
        return false;
    }

    //enum to change player stat
    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    };

    public enum AttributeToChange
    {
        none,
        strength,
        defense,
        intelligence,
        agility
    };

}
