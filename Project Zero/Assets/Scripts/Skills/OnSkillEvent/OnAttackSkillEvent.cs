using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent
{
    public class OnAttackSkillEvent : OnSkillEvent
    {
        protected override void RegisterEvents(Unit owner)
        {
            owner.unitController.OnAttackEndCallback += OnAttack;
        }

        protected override void UnRegisterEvents(Unit owner)
        {
            owner.unitController.OnAttackEndCallback -= OnAttack;
        }

        protected virtual void OnAttack(Unit target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Cast();
        }


    }
}
