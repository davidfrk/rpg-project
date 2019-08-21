using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent
{
    public class OnKillSkillEvent : OnSkillEvent
    {
        protected override void RegisterEvents(Unit owner)
        {
            owner.unitController.OnKillCallback += OnKill;
        }

        protected override void UnRegisterEvents(Unit owner)
        {
            owner.unitController.OnKillCallback -= OnKill;
        }

        protected virtual void OnKill(Unit prey)
        {
            target = prey;

            Cast();
        }
    }
}
