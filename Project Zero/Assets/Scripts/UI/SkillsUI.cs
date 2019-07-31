using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;

public class SkillsUI : MonoBehaviour
{
    public SkillSlotUI skillUIPrefab;
    public Transform skillsTransform;
    List<SkillSlotUI> skillSlots = new List<SkillSlotUI>();
    PlayerController localPlayer;
    Unit selectedUnit;
    SkillsManager castController;

    void Start()
    {
        localPlayer = PlayerController.localPlayer;
    }

    void Update()
    {
        if (selectedUnit != localPlayer.selectedUnit)
        {
            selectedUnit = localPlayer.selectedUnit;

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        foreach (SkillSlotUI skillSlot in skillSlots)
        {
            Destroy(skillSlot.gameObject);
        }
        skillSlots.Clear();

        castController = selectedUnit.GetComponent<SkillsManager>();

        if (castController != null)
        {
            foreach (Skill skill in castController.skills)
            {
                SkillSlotUI skillSlot =  Instantiate<SkillSlotUI>(skillUIPrefab, skillsTransform);
                skillSlot.UpdateUI(skill);
                skillSlots.Add(skillSlot);
            }
        }
    }
}
