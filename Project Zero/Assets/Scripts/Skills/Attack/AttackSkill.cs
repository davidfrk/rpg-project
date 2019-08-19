using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class AttackSkill : Skill
    {
        public override UnitState OnCast()
        {
            Owner.unitController.MoveAttack(Owner.unitController.action.targetUnit);
            return UnitState.None;
        }
    }
}
