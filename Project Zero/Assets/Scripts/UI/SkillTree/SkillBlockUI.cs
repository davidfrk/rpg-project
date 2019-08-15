using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.ProgressionSystem;
using Rpg.Skills;

namespace Rpg.UI {
    public class SkillBlockUI : MonoBehaviour
    {
        public Text tier;
        public Transform skillsTransform;
        public SkillBlockElementUI skillElementPrefab;

        public void UpdateUI(SkillBlock skillBlock)
        {
            tier.text = (skillBlock.tier).ToString();

            foreach (Talent talent in skillBlock.talents)
            {
                SkillBlockElementUI skillUI = Instantiate<SkillBlockElementUI>(skillElementPrefab, skillsTransform);
                skillUI.UpdateUI(talent);
            }
        }
    }
}
