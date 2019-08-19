using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class OnKillSkill : Skill
    {
        protected override void RegisterEvents(Unit owner)
        {
            owner.unitController.OnKillCallback += OnKill;
        }

        protected override void UnRegisterEvents()
        {
            Owner.unitController.OnKillCallback -= OnKill;
        }

        protected virtual void OnKill(Unit prey)
        {
            target = prey;
            OnAction();
        }
    }
}
