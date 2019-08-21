using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent
{
    public class OnCastSkillEvent : OnSkillEvent
    {
        protected override void RegisterEvents(Unit owner)
        {
            skill.OnCastCallback += Cast;
        }

        protected override void UnRegisterEvents(Unit owner)
        {
            skill.OnCastCallback -= Cast;
        }
    }
}
