using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

namespace Rpg.Items
{
    [RequireComponent(typeof(Inventory))]
    public class EquipmentManager : MonoBehaviour
    {
        private Unit unit;
        internal Inventory inventory;
        public List<EquipmentSlot> equipmentSlots;
        public List<Item> startingItems;

        public delegate void PickUpEvent(Item item);
        public event PickUpEvent OnItemPickUpCallback;

        void Awake()
        {
            unit = GetComponent<Unit>();
            inventory = GetComponent<Inventory>();
        }

        void Start()
        {
            foreach(Item item in startingItems)
            {
                PickItem (Instantiate(item));
            }
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
            if (inventory.AddItem(item))
            {
                OnItemPickUpCallback?.Invoke(item);
            }
        }

        public bool HasEquipmentSlotAvailable(Item item)
        {
            Equipment equipment = item as Equipment;
            Equipment.EquipmentType equipmentType = equipment.equipmentType;

            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                EquipmentSlot slot = equipmentSlots[i];

                if (slot.equipment == null && equipmentType == slot.equipmentType)
                {
                    return true;
                }
            }
            return false;
        }

        public int FindEquipmentSlot(Item item)
        {
            Equipment equipment = item as Equipment;
            Equipment.EquipmentType equipmentType = equipment.equipmentType;

            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                EquipmentSlot slot = equipmentSlots[i];

                if (equipmentType == slot.equipmentType)
                {
                    return i;
                }
            }
            Debug.LogError("No compatible equipment slot found");
            return -1;
        }

        public void AddInEquipmentSlot(Item item)
        {
            Equipment equipment = item as Equipment;
            Equipment.EquipmentType equipmentType = equipment.equipmentType;

            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                EquipmentSlot slot = equipmentSlots[i];

                if (slot.equipment == null && equipmentType == slot.equipmentType)
                {
                    equipment.item.SetState(Item.ItemState.InInventory, transform);
                    slot.equipment = equipment;
                    equipmentSlots[i] = slot;

                    //If you found an available equipment slot return
                    Equip(equipment);

                    OnItemPickUpCallback?.Invoke(item);
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

        public Item DropItem(Item.ItemType slotType, int slotIndex)
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
                item.SetState(Item.ItemState.InWorld, null);
                AudioManager.instance.PlaySound(AudioManager.UISound.Drop);
            }
            return item;
        }

        public void SwapItems(Item.ItemType slot1Type, int slot1Index, Item.ItemType slot2Type, int slot2Index)
        {
            if (CanSwap(slot1Type, slot1Index, slot2Type, slot2Index))
            {
                Item item1 = GetItem(slot1Type, slot1Index);
                Item item2 = GetItem(slot2Type, slot2Index);

                SetSlot(slot1Type, slot1Index, null);
                SetSlot(slot2Type, slot2Index, null);

                SetSlot(slot1Type, slot1Index, item2);
                SetSlot(slot2Type, slot2Index, item1);

                //SoundManager.instance.PlaySound(SoundManager.UISound.Swap);
            }
        }

        private bool CanSwap(Item.ItemType slot1Type, int slot1Index, Item.ItemType slot2Type, int slot2Index)
        {
            return isSlotCompatible(slot1Type, slot1Index, GetItem(slot2Type, slot2Index)) &&
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
                Equipment equip = item as Equipment;

                return equipSlot.equipmentType == equip.equipmentType;
            }
        }

        public Item GetItem(Item.ItemType slotType, int slotIndex)
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
                Equipment newEquip = item as Equipment;

                if (newEquip != null && equipmentSlots[slot].equipmentType == newEquip.equipmentType)
                {
                    equipSlot.equipment = newEquip;
                    Equip(newEquip);
                }
            }

            equipmentSlots[slot] = equipSlot;
        }

        public bool HasItem(Item item)
        {
            return inventory.Items.Exists(Item => Item.id == item.id) || equipmentSlots.Exists(EquipmentSlot => EquipmentSlot.equipment?.id == item.id);
        }

        public bool HasItem(Item item, int amount)
        {
            int count = inventory.Items.FindAll(Item => Item?.id == item.id).Count + equipmentSlots.FindAll(EquipmentSlot => EquipmentSlot.equipment?.id == item.id).Count;
            return count >= amount;
        }

        public bool HasItemList(List<ItemQuantity> items)
        {
            foreach(ItemQuantity itemQuantity in items)
            {
                if (!HasItem(itemQuantity.item, itemQuantity.amount))
                {
                    return false;
                }
            }
            return true;
        }

        private bool FindItem(Item item, out Item.ItemType itemType, out int index)
        {
            int itemIndex = inventory.Items.FindIndex(Item => (Item != null && Item.id == item.id));
            if (itemIndex >= 0)
            {
                index = itemIndex;
                itemType = Item.ItemType.Item;
                return true;
            }
            else
            {
                itemIndex = equipmentSlots.FindIndex(EquipmentSlot => (EquipmentSlot.equipment != null && EquipmentSlot.equipment.id == item.id));

                if (itemIndex >= 0)
                {
                    index = itemIndex;
                    itemType = Item.ItemType.Equipment;
                    return true;
                }
                else
                {
                    index = itemIndex;
                    itemType = Item.ItemType.Item;
                    return false;
                }
            }
        }

        private void RemoveItem(Item item)
        {
            Item.ItemType itemType;
            int index;
            if (FindItem(item, out itemType, out index))
            {
                Item oldItem = DropItem(itemType, index);
                Destroy(oldItem.gameObject);
            }
        }

        private void RemoveItem(Item item, int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                RemoveItem(item);
            }
        }

        public void RemoveItemList(List<ItemQuantity> items)
        {
            foreach (ItemQuantity itemQuantity in items)
            {
                RemoveItem(itemQuantity.item, itemQuantity.amount);
            }
        }

        void Equip(Equipment equipment)
        {
            unit.stats.AddStatModifierList(equipment.statBonus, equipment);
            unit.crit.AddCritList(equipment.critBonus, equipment);

            if (equipment.skill != null)
            {
                equipment.skill.Owner = unit;
            }

            AudioManager.instance.PlaySound(AudioManager.UISound.Equip);
        }

        void UnEquip(Equipment equipment)
        {
            unit.stats.RemoveStatModifierList(equipment.statBonus, equipment);
            unit.crit.RemoveAllCritsFromSource(equipment);

            if (equipment.skill != null)
            {
                equipment.skill.Owner = null;
            }
        }
    }
}
