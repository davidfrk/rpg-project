using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    public Transform castTransform;
    public List<Skill> skills;
    public List<Skill> startingSkills;
    private Skill skill;

    private Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
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
        skill.owner = unit;
        skills.Add(skill);
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

    public void OnCastStart(int skillSlot)
    {
        skill = skills[skillSlot];
        skill.Cast(unit, castTransform, Vector3.forward);//ToDo: Atualizar cast position
        unit.PayManaCost(skill.manaCost);
    }

    public void OnCastEnd()
    {
        skill.OnCastEnd();
    }
}
