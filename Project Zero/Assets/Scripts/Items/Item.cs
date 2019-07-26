using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int id;
    public Sprite sprite;
    public ItemType itemType = ItemType.Item;
    public GameObject model;

    public enum ItemType
    {
        Item,
        Key,
        Equipment
    }

    public void SetState(ItemState state)
    {
        if (state == ItemState.InWorld)
        {
            model?.SetActive(true);
        }
        else
        {
            model?.SetActive(false);
        }
    }

    public enum ItemState
    {
        InWorld,
        InInventory
    }
}
