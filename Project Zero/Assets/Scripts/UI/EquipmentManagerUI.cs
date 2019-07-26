using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManagerUI : MonoBehaviour
{
    public Unit selectedUnit;
    public List<ItemSlotUI> equipmentSlots;

    public void Start()
    {
        InitiateSlots();
    }

    private void InitiateSlots()
    {
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            equipmentSlots[i].slotNumber = i;
            equipmentSlots[i].slotType = Item.ItemType.Equipment;
        }
    }

    public void UpdateEquipmentUI()
    {
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            bool foundEquipment = false;
            if (selectedUnit != null)
            {
                EquipmentManager equipmentManager = selectedUnit.GetComponent<EquipmentManager>();

                if (equipmentManager != null)
                {
                    Equipment equipment = equipmentManager.equipmentSlots[i].equipment;
                    if (equipment != null)
                    {
                        equipmentSlots[i].UpdateItemSlot(equipment.item);
                        foundEquipment = true;
                    }
                }
            }
            //if you can't find an item set null
            if (!foundEquipment) equipmentSlots[i].UpdateItemSlot(null);
        }
    }

    //Temporario até ter eventos
    void Update()
    {
        UpdateEquipmentUI();
    }
}
