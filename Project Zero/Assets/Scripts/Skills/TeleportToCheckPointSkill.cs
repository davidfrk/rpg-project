using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class TeleportToCheckPointSkill : Skill
    {
        public override void Cast(Unit owner, Transform castTransform, Unit targetUnit)
        {
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play(true);
            audioSource.Play();
        }

        public override void OnCastEnd()
        {
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            audioSource.Stop();

            Owner.unitController.Teleport(Owner.GetComponent<UnitRespawn>().SpawnPosition, transform.rotation);
            Owner.RegenToFull();
        }
    }
}
