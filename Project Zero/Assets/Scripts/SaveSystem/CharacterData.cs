using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.ProgressionSystem;
using Rpg.Items;

public class CharacterData
{
    public float health;
    public float mana;
    public float exp;
    public int[] statsLevels;
    public int skillPoints;
    public int[] equipments;
    public int[] inventory;
    public float[] position;

    public void GetData(Unit unit)
    {
        //Stats
        health = unit.Health;
        mana = unit.Mana;

        statsLevels = new int[4];
        statsLevels[0] = unit.stats.StrLevel;
        statsLevels[1] = unit.stats.DexLevel;
        statsLevels[2] = unit.stats.IntLevel;
        statsLevels[3] = unit.stats.WillLevel;

        //Level
        ExperienceManager experienceManager = unit.GetComponent<ExperienceManager>();
        if (experienceManager)
        {
            exp = experienceManager.Exp;
        }

        //Equipment
        EquipmentManager equipmentManager = unit.GetComponent<EquipmentManager>();
        if (equipmentManager != null)
        {
            equipments = new int[equipmentManager.equipmentSlots.Count];
            for (int i = 0; i < equipments.Length; i++)
            {
                Equipment equipment = equipmentManager.equipmentSlots[i].equipment;
                if (equipment != null)
                {
                    equipments[i] = equipment.id;
                }
                else
                {
                    equipments[i] = -1;
                }
            }
        }

        //Inventory
        Inventory inventoryManager = unit.GetComponent<Inventory>();
        if (inventoryManager != null)
        {
            inventory = new int[inventoryManager.Items.Count];
            for (int i = 0; i < inventory.Length; i++)
            {
                Item item = inventoryManager.Items[i];
                if (item != null)
                {
                    inventory[i] = item.id;
                }
                else
                {
                    inventory[i] = -1;
                }
            }
        }

        //SkillTree
        SkillTree skillTree = unit.GetComponent<SkillTree>();
        if (skillTree != null)
        {
            skillPoints = skillTree.TotalPoints;
        }

        //Position
        position = new float[3];
        position[0] = unit.transform.position.x;
        position[1] = unit.transform.position.y;
        position[2] = unit.transform.position.z;
    }

    public void Apply(Unit unit)
    {
        //Stats
        unit.stats.StrLevel = statsLevels[0];
        unit.stats.DexLevel = statsLevels[1];
        unit.stats.IntLevel = statsLevels[2];
        unit.stats.WillLevel = statsLevels[3];
        unit.stats.UpdateBaseStats();

        //Level
        ExperienceManager experienceManager = unit.GetComponent<ExperienceManager>();
        if (experienceManager)
        {
            experienceManager.Exp = exp;
        }

        //Equipment
        EquipmentManager equipmentManager = unit.GetComponent<EquipmentManager>();
        if (equipmentManager != null)
        {
            DropAndDestroyAllEquips(equipmentManager);
            for (int i = 0; i < equipments.Length; i++)
            {
                AddEquip(equipmentManager, i, equipments[i]);
            }
        }

        //Inventory
        if (equipmentManager != null)
        {
            DropAndDestroyAllItems(equipmentManager);
            for (int i = 0; i < inventory.Length; i++)
            {
                AddItem(equipmentManager, i, inventory[i]);
            }
        }

        //SkillTree
        SkillTree skillTree = unit.GetComponent<SkillTree>();
        if (skillTree != null)
        {
            skillTree.TotalPoints = skillPoints;
            skillTree.Reset();
        }

        //Position
        unit.unitController.Teleport(new Vector3(position[0], position[1], position[2]), Quaternion.identity);

        //UpdateStats
        unit.UpdateStats();
        unit.Health = health;
        unit.Mana = mana;
    }

    private void AddItem(EquipmentManager equipmentManager, int slot, int itemID)
    {
        if (itemID < 0 || itemID >= GameManager.instance.itemsList.items.Count) return;

        Item item = GameManager.instance.itemsList.items[itemID];

        if (item != null)
        {
            Item newItem = GameObject.Instantiate<Item>(item);
            equipmentManager.AddInInventory(newItem);
        }
    }

    private void DropAndDestroyAllItems(EquipmentManager equipmentManager)
    {
        for (int i = 0; i < equipmentManager.inventory.Items.Count; i++)
        {
            Item item = equipmentManager.DropItem(Item.ItemType.Item, i);
            if (item != null)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }

    private void AddEquip(EquipmentManager equipmentManager, int slot, int itemID)
    {
        if (itemID < 0 || itemID >= GameManager.instance.itemsList.items.Count) return;

        Item item = GameManager.instance.itemsList.items[itemID];

        if (item != null){
            Item newItem = GameObject.Instantiate<Item>(item);
            equipmentManager.AddInEquipmentSlot(newItem);
        }
    }

    private void DropAndDestroyAllEquips(EquipmentManager equipmentManager)
    {
        for (int i = 0; i < equipmentManager.equipmentSlots.Count; i++)
        {
            Item item = equipmentManager.DropItem(Item.ItemType.Equipment, i);
            if (item != null)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }
}
