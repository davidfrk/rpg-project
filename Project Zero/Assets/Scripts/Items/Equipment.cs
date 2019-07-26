using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    internal Item item;
    public EquipmentType equipmentType;
    public UnitStats equipStats;

    void Awake()
    {
        item = GetComponent<Item>();
    }

    public enum EquipmentType
    {
        Weapon,
        Armor,
        Helmet,
        Pants,
        Boots,
        Accessory
    }
}
