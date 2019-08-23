using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class ChannelSkill : Skill
    {
        protected bool active = false;

        public override void OnCastStart(Unit owner, Transform castTransform, Unit targetUnit)
        {
            StartChannel();
            base.OnCastStart(owner, castTransform, targetUnit);
        }

        public override void OnCastStart(Unit owner, Transform castTransform, Vector3 targetPosition)
        {
            StartChannel();
            base.OnCastStart(owner, castTransform, targetPosition);
        }

        public override void OnCastEnd()
        {
            StopChannel();
            base.OnCastEnd();
        }

        public override void Interrupt()
        {
            StopChannel();
            base.Interrupt();
        }

        public virtual void StartChannel()
        {
            if (!active)
            {
                ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                particleSystem.Play(true);

                AudioSource.Play();
                active = true;
            }
        }

        public virtual void StopChannel()
        {
            if (active)
            {
                ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                AudioSource.Stop();
                active = false;
            }
        }
    }
}
