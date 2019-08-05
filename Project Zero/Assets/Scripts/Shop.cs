using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;

public class Shop : MonoBehaviour
{
    public int gold;
    public List<Item> items;

    public void Sell(Item item, PlayerController player)
    {
        if (items.Contains(item) && player.gold >= item.price && player.selectedUnit != null)
        {
            gold += item.price;
            player.gold -= item.price;
            Instantiate<Item>(item, player.selectedUnit.transform.position, Quaternion.identity);
        }
    }
}
