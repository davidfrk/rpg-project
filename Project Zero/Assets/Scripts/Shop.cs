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
            SoundManager.instance.PlaySound(SoundManager.UISound.BuyItem);
        }
    }

    public void Buy(Item item, PlayerController player)
    {
        if (item != null && item.State == Item.ItemState.InWorld && gold >= item.price)
        {
            int price = Mathf.FloorToInt(sellFraction * item.price);
            gold -= price;
            player.gold += price;
            Destroy(item.gameObject);
            SoundManager.instance.PlaySound(SoundManager.UISound.SellItem);
        }
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
