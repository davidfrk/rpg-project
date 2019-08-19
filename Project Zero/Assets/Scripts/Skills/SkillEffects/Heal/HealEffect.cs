using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

namespace Rpg.Skills.Effects
{
    public class HealEffect : SkillEffect
    {
        public Unit.Resource resource;
        public StatBonusType bonusType;
        public float amount;

        protected override void Cast()
        {
            if (bonusType == StatBonusType.Flat)
            {
                skill.Owner.Regen(resource, amount, skill.Owner);
            }
            else
            {
                skill.Owner.Regen(resource, amount * skill.damageOnTarget, skill.Owner);
            }
        }
    }
}