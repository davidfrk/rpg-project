using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Items;
using Rpg.Stats;

namespace Rpg.UI
{
    public class ShopItemTooltip : MonoBehaviour
    {
        public Image icon;
        public Text itemName;
        public Text price;
        public Transform statsTransform;
        public StatsTooltipElement statsPrefab;
        public StatsTooltipGradeElement statsGradePrefab;

        public void UpdateUI(Item item)
        {
            icon.sprite = item.sprite;
            itemName.text = item.gameObject.name;
            price.text = item.price.ToString();

            if (item is Equipment)
            {
                UpdateStatsUI(item);
            }
        }

        private void UpdateStatsUI(Item item)
        {
            Equipment equipment = item as Equipment;

            foreach (StatBonus statBonus in equipment.statBonus)
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
