using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class EquipmentManager : MonoBehaviour
{
    Unit unit;
    internal Inventory inventory;
    public List<EquipmentSlot> equipmentSlots;
    
    void Awake()
    {
        unit = GetComponent<Unit>();
        inventory = GetComponent<Inventory>();
    }

    void Start()
    {
        //UpdateEquipmentStats();
    }

    public void PickItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Equipment:
                {
                    AddInEquipmentSlot(item);
                    break;
                }
            default:
                {
                    AddInInventory(item);
                    break;
                }
        }
    }

    public void AddInInventory(Item item)
    {
        if (!inventory.isFull())
        {
            item.SetState(Item.ItemState.InInventory);
            inventory.AddItem(item);
        }
    }

    public void AddInEquipmentSlot(Item item)
    {
        Equipment equipment = item.GetComponent<Equipment>();
        Equipment.EquipmentType equipmentType = equipment.equipmentType;

        for(int i = 0; i < equipmentSlots.Count; i++)
        {
            EquipmentSlot slot = equipmentSlots[i];

            if (slot.equipment == null && equipmentType == slot.equipmentType)
            {
                equipment.item.SetState(Item.ItemState.InInventory);
                slot.equipment = equipment;
                equipmentSlots[i] = slot;

                //If you found an available equipment slot return
                Equip(equipment);
                //UpdateEquipmentStats();
                return;
            }
        }

        //If not, try to add in inventory
        AddInInventory(item);
    }

    private void RemoveEquipment(int slot)
    {
        EquipmentSlot equipmentSlot = equipmentSlots[slot];
        Equipment equipment = equipmentSlot.equipment;

        if (equipment != null)
        {
            equipmentSlot.equipment = null;
            equipmentSlots[slot] = equipmentSlot;
            UnEquip(equipment);
        }
    }

    public void DropItem(Item.ItemType slotType, int slotIndex)
    {
        Item item;
        if (slotType == Item.ItemType.Equipment)
        {
            item = equipmentSlots[slotIndex].equipment?.item;
            RemoveEquipment(slotIndex);

            //UpdateEquipmentStats();
        }
        else
        {
            item = inventory.Items[slotIndex];
            inventory.RemoveItem(slotIndex);
        }

        if (item != null)
        {
            item.transform.position = transform.position;
            item.SetState(Item.ItemState.InWorld);
        }
    }

    public void SwapItems(Item.ItemType slot1Type, int slot1Index, Item.ItemType slot2Type, int slot2Index)
    {
        if (CanSwap(slot1Type, slot1Index, slot2Type, slot2Index))
        {
            Item item1 = GetItem(slot1Type, slot1Index);
            Item item2 = GetItem(slot2Type, slot2Index);

            SetSlot(slot1Type, slot1Index, item2);
            SetSlot(slot2Type, slot2Index, item1);

            //UpdateEquipmentStats();
        }
    }

    private bool CanSwap(Item.ItemType slot1Type, int slot1Index, Item.ItemType slot2Type, int slot2Index)
    {
        return  isSlotCompatible(slot1Type, slot1Index, GetItem(slot2Type, slot2Index)) &&
                isSlotCompatible(slot2Type, slot2Index, GetItem(slot1Type, slot1Index));
    }

    private bool isSlotCompatible(Item.ItemType slotType, int slotIndex, Item item)
    {
        if (item == null || slotType != Item.ItemType.Equipment)
        {
            return true;
        }
        else
        {
            EquipmentSlot equipSlot = equipmentSlots[slotIndex];
            Equipment equip = item.GetComponent<Equipment>();

            return equipSlot.equipmentType == equip.equipmentType;
        }
    }

    private Item GetItem(Item.ItemType slotType, int slotIndex)
    {
        if (slotType == Item.ItemType.Equipment)
        {
            return equipmentSlots[slotIndex].equipment?.item;
        }
        else
        {
            return inventory.Items[slotIndex];
        }
    }

    private void SetSlot(Item.ItemType slotType, int slotIndex, Item item)
    {
        if (slotType == Item.ItemType.Equipment)
        {
            SetEquipmentSlot(slotIndex, item);
        }
        else
        {
            inventory.Items[slotIndex] = item;
        }
    }

    private void SetEquipmentSlot(int slot, Item item)
    {
        EquipmentSlot equipSlot = equipmentSlots[slot];
        if (equipSlot.equipment != null)
        {
            UnEquip(equipSlot.equipment);
            equipSlot.equipment = null;
        }

        if (item != null)
        {
            Equipment newEquip = item.GetComponent<Equipment>();

            if (newEquip != null && equipmentSlots[slot].equipmentType == newEquip.equipmentType)
            {
                equipSlot.equipment = newEquip;
                Equip(newEquip);
            }
        }

        equipmentSlots[slot] = equipSlot;
    }
    /*
    public void UpdateEquipmentStats()
    {
        foreach(EquipmentSlot slot in equipmentSlots)
        {
            if (slot.equipment != null)
            {
                Equip(slot.equipment);
            }
        }
    }*/

    void Equip(Equipment equipment)
    {
        foreach(StatBonus statBonus in equipment.statBonus)
        {
            unit.stats.AddStatModifier(statBonus.stat, statBonus.value, statBonus.statModType, equipment);
        }
    }

    void UnEquip(Equipment equipment)
    {
        foreach (StatBonus statBonus in equipment.statBonus)
        {
            unit.stats.RemoveStatModifier(statBonus.stat, equipment);
        }
    }
}
