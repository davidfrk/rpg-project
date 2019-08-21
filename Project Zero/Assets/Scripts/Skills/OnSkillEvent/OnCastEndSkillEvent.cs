using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent
{
    public class OnCastEndSkillEvent : OnSkillEvent
    {
        protected override void RegisterEvents(Unit owner)
        {
            skill.OnCastEndCallback += Cast;
        }

        protected override void UnRegisterEvents(Unit owner)
        {
            skill.OnCastEndCallback -= Cast;
        }
    }
}
