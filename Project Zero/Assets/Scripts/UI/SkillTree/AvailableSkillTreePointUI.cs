using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.ProgressionSystem;

namespace Rpg.UI {
    public class AvailableSkillTreePointUI : MonoBehaviour
    {
        public GameObject availableSkillPointUI;

        void Update()
        {
            SkillTree skillTree = PlayerController.localPlayer.MainUnit?.GetComponent<SkillTree>();

            if (skillTree != null)
            {
                if (skillTree.SkillPoints > 0)
                {
                    availableSkillPointUI.SetActive(true);
                }
                else
                {
                    availableSkillPointUI.SetActive(false);
                }
            }
        }
    }
}
