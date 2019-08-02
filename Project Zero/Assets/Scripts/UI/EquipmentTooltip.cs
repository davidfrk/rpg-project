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
        public StatsTooltipGradeElement statsGradePrefab;

        internal Equipment equipment;
        private List<StatsTooltipElement> statsTooltipElements;
        private List<StatsTooltipGradeElement> statsGradeElements;

        public void UpdateUI(Equipment equipment)
        {
            this.equipment = equipment;
            statsName.text = equipment.name;

            foreach(StatBonus statBonus in equipment.statBonus)
            {
                AddStatsUI(statBonus);
            }
        }

        private void AddStatsUI(StatBonus statBonus)
        {
            if (statBonus.dependencies.Count == 0)
            {
                StatsTooltipElement statTooltip = Instantiate<StatsTooltipElement>(statsPrefab, statsTransform);
                statTooltip.UpdateUI(statBonus);
            }
            else
            {
                foreach (StatModifier.Dependency dependency in statBonus.dependencies)
                {
                    StatsTooltipGradeElement gradeTooltip = Instantiate<StatsTooltipGradeElement>(statsGradePrefab, statsTransform);
                    gradeTooltip.UpdateUI(dependency);
                }
            }
        }
    }
}
