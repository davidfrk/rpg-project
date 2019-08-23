using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/StompEffect", order = 1)]
    public class StompEffect : SkillEffect
    {
        public float castDistance;
        public float radius;
        public float damage;
        public DamageType damageType;
        public AudioClip effectSound;
        public GameObject effectPrefab;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            Vector3 position = skill.Owner.transform.position + castDistance * skill.Owner.transform.forward + 0.2f * Vector3.up;
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