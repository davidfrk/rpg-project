using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Stats
{
    [System.Serializable]
    public struct StatBonus: IStat
    {
        public StatType stat;
        public StatModType statModType;
        public float value;
        public List<StatModifier.Dependency> dependencies;

        public string GetName()
        {
            if (stat == StatType.DamageTakenMultiplier)
            {
                return "DmgBlock";
            }
            else
            {
                return stat.ToString();
            }
        }

        public string ValueToString()
        {
            if (statModType == StatModType.Flat)
            {
                return value.ToString("F0");
            }
            else
            {
                if (stat == StatType.DamageTakenMultiplier)
                {
                    return (100f * -value).ToString("F0") + "%";
                }
                else
                {
                    return (100f * value).ToString("F0") + "%";
                }
            }
        }
    }

    public enum StatBonusType
    {
        Flat,
        Mult,
    }
}
