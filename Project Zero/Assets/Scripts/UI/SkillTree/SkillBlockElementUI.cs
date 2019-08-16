using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rpg.Skills;
using Rpg.ProgressionSystem;

namespace Rpg.UI {
    public class SkillBlockElementUI : MonoBehaviour, IPointerDownHandler
    {
        public Image icon;
        public Text value;
        private SkillTreeUI skillTreeUI;
        private Talent talent;

        void Awake()
        {
            skillTreeUI = GetComponentInParent<SkillTreeUI>();
        }

        public void UpdateUI(Talent talent)
        {
            this.talent = talent;
            icon.sprite = talent.icon;
            if (talent.Acquired)
            {
                icon.color = Color.white;
            }
            else
            {
                icon.color = Color.gray;
            }

            if (talent.statBonus[0].statModType == Stats.StatModType.Flat)
            {
                value.text = talent.statBonus[0].stat.ToString() + " " + talent.statBonus[0].value.ToString("F0");
            }
            else
            {
                value.text = talent.statBonus[0].stat.ToString() + " " + (talent.statBonus[0].value * 100).ToString("F0") + "%";
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                skillTreeUI.OnMouseRightClickDown(talent);
                UpdateUI(talent);
            }
        }
    }
}
