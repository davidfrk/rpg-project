using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;

public class DropSystem : MonoBehaviour
{
    public List<DropChance> dropList;

    void Awake()
    {
        UnitController unitControler = GetComponent<UnitController>();
        unitControler.OnDeathCallback += Drop;
    }

    void Drop(Unit killer)
    {
        Item dropItem = null;
        float random = Random.value;
        float sum = 0f;

        //choosing only one item
        foreach (DropChance dropChance in dropList)
        {
            sum += dropChance.chance;
            if (sum > random)
            {
                dropItem = dropChance.item;
                break;
            }
        }
        
        if (dropItem != null)
        {
            Drop(dropItem);
        }
    }

    void Drop (Item item)
    {
        Instantiate(item, transform.position, Quaternion.identity);
    }
}

[System.Serializable]
public struct DropChance
{
    public float chance;
    public Item item;
}
