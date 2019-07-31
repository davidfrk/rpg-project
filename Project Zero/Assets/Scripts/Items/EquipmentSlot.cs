using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Items
{

    [System.Serializable]
    public struct EquipmentSlot
    {
        public Equipment.EquipmentType equipmentType;
        public Equipment equipment;
    }
}
