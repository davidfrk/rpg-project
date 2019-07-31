using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

public class FocusSkill : Skill
{
    [Space(10)]
    public float strBuff = 20;
    public float hpRegenBuff = 5f;
    public float duration = 10f;

    public override void Cast(Unit owner, Transform castTransform, Vector3 targetPosition)
    {
        owner.stats.Str.AddModifier(new StatModifier(strBuff, StatModType.Flat, (int) StatModType.Flat, this));
        owner.stats.HpRegen.AddModifier(new StatModifier(hpRegenBuff, StatModType.Flat, (int)StatModType.Flat, this));

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play(true);

        Invoke("Stop", duration);
    }

    public override void OnCastEnd()
    {
        
    }

    private void Stop()
    {
        owner.stats.Str.RemoveAllModifiersFromSource(this);
        owner.stats.HpRegen.RemoveAllModifiersFromSource(this);
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
