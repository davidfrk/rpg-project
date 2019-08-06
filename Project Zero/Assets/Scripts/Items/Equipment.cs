using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills;

namespace Rpg.Items
{
    public class Equipment : Item
    {
        [Space(10)]
        public EquipmentType equipmentType;
        public List<StatBonus> statBonus;
        public Skill skill;

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
