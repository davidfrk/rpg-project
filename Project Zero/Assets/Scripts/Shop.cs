using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;

public class Shop : MonoBehaviour
{
    static public float range = 10f;
    public int gold;
    public float sellFraction;
    public List<Item> items;

    public void Sell(Item item, PlayerController player)
    {
        if (items.Contains(item) && player.gold >= item.price && player.selectedUnit != null)
        {
            gold += item.price;
            player.gold -= item.price;
            Item newItem = Instantiate<Item>(item, player.selectedUnit.transform.position, Quaternion.identity);
            player.selectedUnit.unitController.MoveToPickItem(newItem);
            AudioManager.instance.PlaySound(AudioManager.UISound.BuyItem);
        }
    }

    public void Buy(Item item, PlayerController player)
    {
        if (item != null && CanBuy(item, player))
        {
            int price = SellPrice(item, player);
            gold -= price;
            player.gold += price;
            Destroy(item.gameObject);
            AudioManager.instance.PlaySound(AudioManager.UISound.SellItem);
        }
    }

    public bool CanBuy(Item item, PlayerController player)
    {
        return gold >= SellPrice(item, player);
    }

    public int SellPrice(Item item, PlayerController player)
    {
        return Mathf.FloorToInt(sellFraction * item.price);
    }

    static public Shop FindShopInRange(Vector3 position)
    {
        Collider[] shopsInRange = Physics.OverlapSphere(position, range, LayerMask.GetMask("Shop"));
        foreach (Collider shopCollider in shopsInRange)
        {
            //ToDo: Pegar o mais proximo
            return shopCollider.GetComponent<Shop>();
        }
        return null;
    }
}
