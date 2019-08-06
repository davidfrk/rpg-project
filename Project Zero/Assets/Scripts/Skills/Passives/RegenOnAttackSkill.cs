using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class RegenOnAttackSkill : Skill
    {
        public Unit.Resource resource;
        public float amount;

        protected override void RegisterEvents(Unit owner)
        {
            owner.unitController.OnAttackEndCallback += OnAttack;
        }

        protected override void UnRegisterEvents()
        {
            Owner.unitController.OnAttackEndCallback -= OnAttack;
        }

        protected virtual void OnAttack(Unit target, float damage)
        {
            Owner.Regen(resource, amount, Owner);
        }
    }
}
