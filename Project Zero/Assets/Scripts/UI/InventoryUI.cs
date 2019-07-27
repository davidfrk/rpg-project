using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Unit selectedUnit;
    public List<ItemSlotUI> ItemSlots;

    public void Start()
    {
        InitiateSlots();
    }

    private void InitiateSlots(){
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSlots[i].slotNumber = i;
            ItemSlots[i].slotType = Item.ItemType.Item;
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (selectedUnit != null)
            {
                Inventory inventory = selectedUnit.GetComponent<Inventory>();

                if (inventory != null)
                {
                    ItemSlots[i].UpdateItemSlot(inventory.Items[i]);
                }
                else
                {
                    ItemSlots[i].UpdateItemSlot(null);
                }
            }
            else
            {
                ItemSlots[i].UpdateItemSlot(null);
            }
        }
    }

    //Temporario até ter eventos
    void Update()
    {
        selectedUnit = UIController.instance.selectedUnit;
        UpdateInventory();
    }
}
