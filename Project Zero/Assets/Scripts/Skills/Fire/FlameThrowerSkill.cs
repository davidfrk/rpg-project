using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class FlameThrowerSkill : ChannelSkill
    {
        [Space(10)]
        public float damageInterval = 0.25f;
        public float damage = 10f;
        public float intMult = 0.5f;
        public float durationAfterCastEnd = 3;
        public float manaCostPerSecMult = 0.5f;
        public Vector3 boxDimensions;

        private float lastDamageTick = 0f;

        public override void OnCastStart(Unit owner, Transform castTransform, Vector3 targetPosition)
        {
            transform.SetParent(castTransform, false);
            transform.rotation = owner.transform.rotation;

            base.OnCastStart(owner, castTransform, targetPosition);
        }

        void Update()
        {
            if (active)
            {
                if ((Time.time - lastDamageTick) > damageInterval)
                {
                    lastDamageTick = Time.time;
                    float damage = (this.damage + intMult * Owner.stats.Int.Value) * damageInterval;
                    Owner.PayManaCost(damage * manaCostPerSecMult);
                    DamageTick(Owner, damage);

                    if (Owner.Mana <= 0f)
                    {
                        StopChannel();
                    }
                }
            }
        }

        public virtual void DamageTick(Unit owner, float damage)
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
    }
}

