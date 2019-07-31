using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class FlameThrowerSkill : Skill
    {
        [Space(10)]
        public float damageInterval = 0.25f;
        public float damage = 10f;
        public float durationAfterCastEnd = 3;
        public Vector3 boxDimensions;

        private bool active = false;
        private float lastDamageTick = 0f;

        public override void Cast(Unit owner, Transform castTransform, Vector3 targetPosition)
        {
            this.owner = owner;
            transform.SetParent(castTransform, false);
            transform.rotation = owner.transform.rotation;
            transform.GetChild(0).gameObject.SetActive(true);
            active = true;

            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play(true);
        }

        public override void OnCastEnd()
        {
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            active = false;
        }

        void Update()
        {
            if (active)
            {
                if ((Time.time - lastDamageTick) > damageInterval)
                {
                    lastDamageTick = Time.time;
                    DamageTick(owner);
                }
            }
        }

        public virtual void DamageTick(Unit owner)
        {
            Vector3 center = owner.transform.position + owner.transform.forward * boxDimensions.z + owner.transform.up * boxDimensions.y;
            Collider[] unitColliders = FindUnitsInBox(center, boxDimensions, owner.transform.rotation);

            foreach (Collider unitCollider in unitColliders)
            {
                Unit unit = unitCollider.GetComponent<Unit>();
                if (unit != owner)
                {
                    unit.TakeDamage(damage, DamageType.Magic, owner);
                }
            }
        }

        public override void Interrupt()
        {
            if (active) OnCastEnd();
        }
    }
}

