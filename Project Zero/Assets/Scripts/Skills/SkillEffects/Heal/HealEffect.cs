using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/HealEffect", order = 1)]
    public class HealEffect : SkillEffect
    {
        public Unit.Resource resource;
        public StatBonusType bonusType;
        public float amount;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            if (bonusType == StatBonusType.Flat)
            {
                skill.Owner.Regen(resource, amount, skill.Owner);
            }
            else
            {
                skill.Owner.Regen(resource, amount * skillEvent.damage, skill.Owner);
            }
        }
    }
}