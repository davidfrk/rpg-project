using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

namespace Rpg.Skills
{

    public class BuffSkill : Skill
    {
        public float duration = 10f;

        public override void OnCastStart(Unit owner, Transform castTransform, Unit targetUnit)
        {
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play(true);
            audioSource.Play();

            StartCoroutine(Stop(targetUnit, duration));

            base.OnCastStart(owner,castTransform,targetUnit);
        }

        IEnumerator Stop(Unit targetUnit, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
