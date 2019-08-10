using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;

public class SkillsManager : MonoBehaviour
{
    public Transform castTransform;
    public List<Skill> skills;
    public List<Skill> startingSkills;
    private Skill skill;

    private Unit unit;
    private UnitController unitController;

    void Awake()
    {
        unit = GetComponent<Unit>();
        unitController = GetComponent<UnitController>();
        unitController.OnDeathCallback += OnDeath;
    }

    void Start()
    {
        foreach(Skill skill in startingSkills)
        {
            AddSkill(Instantiate<Skill>(skill, transform, false));
        }
    }

    public void AddSkill(Skill skill)
    {
        skill.Owner = unit;
        skills.Add(skill);
        skill.key = GetKey(skills.Count -1);
    }

    private string GetKey(int skillSlot)
    {
        switch (skillSlot)
        {
            case 0: return "Q";
            case 1: return "W";
            case 2: return "E";
            case 3: return "R";
            default: return "B";
        }
    }

    public bool CanCast(int skillSlot)
    {
        if (skillSlot >= skills.Count)
        {
            return false;
        }
        else{
            return CanCast(skills[skillSlot]);
        }
    }

    public bool CanCast(Skill skill)
    {
        return unit.Mana >= skill.manaCost;
    }

    public float GetCastRange(int skillSlot)
    {
        if (skillSlot >= skills.Count)
        {
            return 0;
        }
        else
        {
            return skills[skillSlot].castRange;
        }
    }

    public void OnCastStart()
    {
        skill = unitController.action.skill;
        if (skill == null) return;

        if (unitController.action.targetUnit != null)
        {
            skill.Cast(unit, castTransform, unitController.action.targetUnit);
        }
        else
        {
            skill.Cast(unit, castTransform, Vector3.forward);//ToDo: Atualizar cast position
        }
        unit.PayManaCost(skill.manaCost);
    }

    public void OnCastEnd()
    {
        skill?.OnCastEnd();
    }

    private void OnDeath(Unit killer)
    {
        if (skill != null){
            skill.Interrupt();
        }
    }
}
