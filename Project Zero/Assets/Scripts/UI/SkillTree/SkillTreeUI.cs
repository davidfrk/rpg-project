using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.ProgressionSystem;

namespace Rpg.UI {
    public class SkillTreeUI : MonoBehaviour
    {
        public SkillBlockUI skillBlockPrefab;
        public Transform skillBlockTransform;
        public Text availablePointsText;

        private List<SkillBlockUI> blocks = new List<SkillBlockUI>();
        private SkillTree skillTree;
        private bool isActive = false;

        public void Open()
        {
            skillTree = PlayerController.localPlayer.MainUnit.GetComponent<SkillTree>();
            if (skillTree != null)
            {
                UpdateUI(skillTree);
            }

            gameObject.SetActive(true);
            isActive = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            isActive = false;
        }

        public void Toggle()
        {
            if (isActive)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void UpdateUI(SkillTree skillTree)
        {
            foreach(SkillBlockUI skillBlock in blocks)
            {
                Destroy(skillBlock.gameObject);
            }
            blocks.Clear();

            foreach(SkillBlock skillBlock in skillTree.skillBlocks)
            {
                var blockUI = Instantiate<SkillBlockUI>(skillBlockPrefab, skillBlockTransform);
                blockUI.UpdateUI(skillBlock);

                blocks.Add(blockUI);
            }

            UpdateSkillPoints();
        }

        private void UpdateSkillPoints()
        {
            availablePointsText.text = skillTree.SkillPoints.ToString();
        }

        public void OnMouseRightClickDown(Talent talent)
        {
            skillTree.Buy(talent);
            UpdateSkillPoints();
        }
    }
}
