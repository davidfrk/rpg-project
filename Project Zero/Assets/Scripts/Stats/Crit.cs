using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Stats
{
    public class Crit
    {
        public float procChance;
        public float criticalDamage;
        public object source;

        public Crit(CritBonus crit, object source)
        {
            procChance = crit.procChance;
            criticalDamage = crit.criticalDamage;
            this.source = source;
        }
    }

    [System.Serializable]
    public struct CritBonus
    {
        public float procChance;
        public float criticalDamage;
    }
}
