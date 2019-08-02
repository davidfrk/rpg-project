using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Stats;

namespace Rpg.UI
{
    public class StatsTooltipElement : MonoBehaviour
    {
        public Text statName;
        public Text statValue;

        public void UpdateUI(StatBonus statBonus)
        {
            statName.text = statBonus.stat.ToString();
            statValue.text = statBonus.value.ToString("F0");
        }
    }
}
