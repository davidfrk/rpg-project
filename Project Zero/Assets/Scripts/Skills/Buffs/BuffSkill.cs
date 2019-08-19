using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

namespace Rpg.Skills
{

    public class BuffSkill : Skill
    {
        [Space(10)]
        public List<StatBonus> StatBonus;
        public float duration = 10f;

        public override void OnCastStart(Unit owner, Transform castTransform, Unit targetUnit)
        {
            targetUnit.stats.AddStatModifierList(StatBonus, this);
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play(true);
            audioSource.Play();

            StartCoroutine(Stop(targetUnit, duration));
        }

        public override void OnCastEnd()
        {

        }

        IEnumerator Stop(Unit targetUnit, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            targetUnit.stats.RemoveStatModifierList(StatBonus, this);
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
