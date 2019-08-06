using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class RegenOnKillSkill : Skill
    {
        public Unit.Resource resource;
        public float amount;

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
            Owner.Regen(resource, amount, Owner);
        }
    }
}
