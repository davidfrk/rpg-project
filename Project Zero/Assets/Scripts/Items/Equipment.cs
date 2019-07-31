using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

namespace Rpg.Items
{
    public class Equipment : MonoBehaviour
    {
        internal Item item;
        public EquipmentType equipmentType;
        public List<StatBonus> statBonus;

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
}
