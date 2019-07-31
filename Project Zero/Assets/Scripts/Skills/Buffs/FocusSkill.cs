using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

namespace Rpg.Skills
{

    public class FocusSkill : Skill
    {
        [Space(10)]
        public float strBuff = 20;
        public float hpRegenBuff = 5f;
        public float duration = 10f;

        public override void Cast(Unit owner, Transform castTransform, Unit targetUnit)
        {
            targetUnit.stats.Str.AddModifier(new StatModifier(strBuff, StatModType.Flat, (int)StatModType.Flat, this));
            targetUnit.stats.HpRegen.AddModifier(new StatModifier(hpRegenBuff, StatModType.Flat, (int)StatModType.Flat, this));

            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play(true);

            StartCoroutine(Stop(targetUnit, duration));
        }

        public override void OnCastEnd()
        {

        }

        IEnumerator Stop(Unit targetUnit, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            targetUnit.stats.Str.RemoveAllModifiersFromSource(this);
            targetUnit.stats.HpRegen.RemoveAllModifiersFromSource(this);
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
