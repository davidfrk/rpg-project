using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg.Skills
{
    public class JumpAttackSkill : Skill
    {
        public override UnitState OnCast()
        {
            Owner.unitController.movementController.MoveCloseToPosition(Owner.unitController.action.targetPosition, 1f);
            return UnitState.Casting;
        }
    }
}
