using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Stats
{
    [System.Serializable]
    public struct StatBonus
    {
        public StatType stat;
        public StatModType statModType;
        public float value;
    }
}
