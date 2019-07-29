using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastController : MonoBehaviour
{
    public Transform castTransform;
    public List<Skill> skills;
    private Skill skill;

    private Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
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
        skill = Instantiate<Skill>(skills[skillSlot]);
        skill.Cast(unit, castTransform, Vector3.forward);
        unit.PayManaCost(skill.manaCost);
    }

    public void OnCastEnd()
    {
        skill.OnCastEnd();
    }
}
