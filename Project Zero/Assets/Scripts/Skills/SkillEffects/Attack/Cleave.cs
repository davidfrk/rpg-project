using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/Cleave", order = 1)]
    public class Cleave : SkillEffect
    {
        public float radius;
        public float damageMult;
        public DamageType damageType;
        public GameObject effectPrefab;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            Collider[] unitColliders = Skill.FindUnitsInSphere(skill.Owner.transform.position + radius * skill.Owner.transform.forward, radius);

            foreach (Collider unitCollider in unitColliders)
            {
                Unit unit = unitCollider.GetComponent<Unit>();
                if (unit != skill.Owner && unit != skillEvent.target)
                {
                    ApplyEffect(skillEvent, skill, unit);
                }
            }

            Instantiate(effectPrefab, skill.Owner.transform.position, skill.Owner.transform.rotation);
        }

        private void ApplyEffect(OnSkillEvent skillEvent, Skill skill, Unit target)
        {
            target.TakeDamage(damageMult * skillEvent.damage, damageType, skill.Owner);
        }
    }
}
