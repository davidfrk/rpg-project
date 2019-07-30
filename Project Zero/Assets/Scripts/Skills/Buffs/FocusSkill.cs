using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSkill : Skill
{
    [Space(10)]
    public float strBuff = 20;
    public float hpRegenBuff = 5f;
    public float duration = 10f;

    public override void Cast(Unit owner, Transform castTransform, Vector3 targetPosition)
    {
        //owner.unitStats.Str.AddBuff(strBuff, duration);
        //owner.hpRegen.AddBuff( hpRegenBuff, duration);

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play(true);

        Invoke("Stop", duration);
    }

    public override void OnCastEnd()
    {
        
    }

    private void Stop()
    {
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
