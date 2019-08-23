using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/HealToFullEffect")]
    public class HealToFullEffect : SkillEffect
    {
        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            skill.Owner.RegenToFull();
        }
    }
}