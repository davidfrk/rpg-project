using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent
{
    public class OnActionSkillEvent : OnSkillEvent
    {
        protected override void RegisterEvents(Unit owner)
        {
            skill.OnActionCallback += Cast;
        }

        protected override void UnRegisterEvents(Unit owner)
        {
            skill.OnActionCallback -= Cast;
        }
    }
}
