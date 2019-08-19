using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.Effects
{
    public class Cleave : SkillEffect
    {
        public float radius;
        public float damageMult;
        public DamageType damageType;
        
        protected override void Cast()
        {
            Collider[] unitColliders = Skill.FindUnitsInSphere(skill.Owner.transform.position + radius * skill.Owner.transform.forward, radius);

            foreach (Collider unitCollider in unitColliders)
            {
                Unit unit = unitCollider.GetComponent<Unit>();
                if (unit != skill.Owner && unit != skill.target)
                {
                    ApplyEffect(unit);
                }
            }
        }

        private void ApplyEffect(Unit target)
        {
            target.TakeDamage(damageMult * skill.damageOnTarget, damageType, skill.Owner);
        }
    }
}
