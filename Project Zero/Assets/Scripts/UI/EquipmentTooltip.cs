using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Stats;
using Rpg.Items;

namespace Rpg.UI
{
    public class EquipmentTooltip : MonoBehaviour
    {
        public Text statsName;
        public Transform statsTransform;
        public StatsTooltipElement statsPrefab;
        //public StatsTooltipGradeElement statsGradePrefab;

        internal Equipment equipment;
        //private List<StatsTooltipElement> statsTooltipElements;
        //private List<StatsTooltipGradeElement> statsGradeElements;

        public void UpdateUI(Equipment equipment)
        {
            this.equipment = equipment;
            statsName.text = equipment.name;

            foreach(StatBonus statBonus in equipment.statBonus)
            {
                AddStatsUI(statBonus, statsPrefab, statsTransform);
            }

            foreach(CritBonus critBonus in equipment.critBonus)
            {
                AddStatsUI(critBonus, statsPrefab, statsTransform);
            }
        }

        public static void AddStatsUI(StatBonus statBonus, StatsTooltipElement statsUIPrefab, Transform statsTransform)
        {
            if (statBonus.dependencies.Count == 0)
            {
                StatsTooltipElement statTooltip = Instantiate<StatsTooltipElement>(statsUIPrefab, statsTransform);
                statTooltip.UpdateUI(statBonus);
            }
            else
            {
                foreach (StatModifier.Dependency dependency in statBonus.dependencies)
                {
                    StatsTooltipElement statTooltip = Instantiate<StatsTooltipElement>(statsUIPrefab, statsTransform);
                    statTooltip.UpdateUI(dependency);
                }
            }
        }

        public static void AddStatsUI(CritBonus critBonus, StatsTooltipElement statsUIPrefab, Transform statsTransform)
        {
            StatsTooltipElement critDamageTooltip = Instantiate<StatsTooltipElement>(statsUIPrefab, statsTransform);
            critDamageTooltip.UpdateUI("CritMult", PercentValueToString(critBonus.criticalDamage));

            StatsTooltipElement critChangeTooltip = Instantiate<StatsTooltipElement>(statsUIPrefab, statsTransform);
            critChangeTooltip.UpdateUI("ProcChance", PercentValueToString(critBonus.procChance));
        }

        private static string PercentValueToString(float value)
        {
            return (100f * value).ToString("F0") + "%";
        }
    }
}
