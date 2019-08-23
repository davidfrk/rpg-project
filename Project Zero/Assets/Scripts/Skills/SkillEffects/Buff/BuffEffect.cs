using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/BuffEffect", order = 1)]
    public class BuffEffect : SkillEffect
    {
        public float duration = 10f;
        public List<StatBonus> StatBonus;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            skill.TargetUnit.stats.AddStatModifierList(StatBonus, this);

            skill.StartCoroutine(Stop(skill.TargetUnit, duration));
        }

        IEnumerator Stop(Unit targetUnit, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            targetUnit.stats.RemoveStatModifierList(StatBonus, this);
        }
    }
}