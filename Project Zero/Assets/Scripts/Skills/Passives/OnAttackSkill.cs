using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class OnAttackSkill : Skill
    {
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
            this.target = target;
            this.damageOnTarget = damage;
            OnAction();
        }
    }
}
