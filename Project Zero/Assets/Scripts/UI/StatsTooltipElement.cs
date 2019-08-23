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

        public void UpdateUI(IStat stat)
        {
            statName.text = stat.GetName();
            statValue.text = stat.ValueToString();
        }

        public void UpdateUI(string name, string value)
        {
            statName.text = name;
            statValue.text = value;
        }
    }
}
