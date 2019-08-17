using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class AttackSkill : Skill
    {
        public override void OnCastBegin()
        {
            Owner.unitController.MoveAttack(Owner.unitController.action.targetUnit);
        }
    }
}
