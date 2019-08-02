using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Stats;

namespace Rpg.UI
{
    public class StatsTooltipGradeElement : MonoBehaviour
    {
        public Text statName;
        public Text statGrade;

        public void UpdateUI(StatModifier.Dependency dependency)
        {
            statName.text = dependency.StatType.ToString();
            statGrade.text = GetGrade(dependency.Value);
        }

        private string GetGrade(float value)
        {
            int grade = Mathf.FloorToInt(value / 0.5f);

            if (grade <= 0) return "F";
            switch (grade)
            {
                case 0: return "F";
                case 1: return "E";
                case 2: return "D";
                case 3: return "C";
                case 4: return "B";
                case 5: return "A";
                case 6: return "S";
                default: return "S+";
            }
        }
    }
}
