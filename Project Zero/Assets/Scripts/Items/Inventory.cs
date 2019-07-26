using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items;
    internal int count;

    public void AddItem(Item item)
    {
        if (!isFull())
        {
            int slot = FindEmptySlot();
            Items[slot] = item;
            count++;
        }
    }

    public void RemoveItem(int slot)
    {
        if (Items[slot] != null)
        {
            Items[slot] = null;
            count--;
        }
    }

    public bool isFull()
    {
        return count >= Items.Count;
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
            {
                return i;
            }
        }

        Debug.LogError("Trying to add item in a full inventory");
        return -1;
    }

    private int CountItems()
    {
        int count = 0;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null)
            {
                count++;
            }
        }

        return count;
    }

    void Start()
    {
        count = CountItems();
    }
}
