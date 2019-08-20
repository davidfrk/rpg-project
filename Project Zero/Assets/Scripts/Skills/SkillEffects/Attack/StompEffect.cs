using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/StompEffect", order = 1)]
    public class StompEffect : SkillEffect
    {
        public float radius;
        public float damage;
        public DamageType damageType;
        public AudioClip effectSound;
        public GameObject effectPrefab;

        protected override void Cast(Skill skill)
        {
            Vector3 position = skill.Owner.transform.position + radius * skill.Owner.transform.forward + 0.2f * Vector3.up;
            Collider[] unitColliders = Skill.FindUnitsInSphere(position, radius);

            foreach (Collider unitCollider in unitColliders)
            {
                Unit unit = unitCollider.GetComponent<Unit>();
                if (unit != skill.Owner)
                {
                    ApplyEffect(skill, unit);
                }
            }

            skill.Owner.unitController.audioManager.Play(effectSound);
            Instantiate(effectPrefab, position , Quaternion.identity);
        }

        private void ApplyEffect(Skill skill, Unit target)
        {
            target.TakeDamage(damage, damageType, skill.Owner);
        }
    }
}