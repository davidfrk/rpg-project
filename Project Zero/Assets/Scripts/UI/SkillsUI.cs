using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rpg.Skills;

public class SkillsUI : MonoBehaviour, ISkillManagerUI
{
    public SkillSlotUI skillUIPrefab;
    public Transform skillsTransform;
    public SkillTooltip skillTooltip;

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
                skillSlot.UpdateUI(skill, KeyCode.Q);
                skillSlots.Add(skillSlot);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData, SkillSlotUI skillSlot)
    {
        skillTooltip.Show(eventData.position, skillSlot.Skill);
    }

    public void OnPointerExit(PointerEventData eventData, SkillSlotUI skillSlot)
    {
        skillTooltip.Hide();
    }
}
