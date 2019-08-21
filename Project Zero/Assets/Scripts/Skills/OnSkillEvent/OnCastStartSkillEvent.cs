using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent
{
    public class OnCastStartSkillEvent : OnSkillEvent
    {
        protected override void RegisterEvents(Unit owner)
        {
            skill.OnCastStartCallback += Cast;
        }

        protected override void UnRegisterEvents(Unit owner)
        {
            skill.OnCastStartCallback -= Cast;
        }
    }
}
