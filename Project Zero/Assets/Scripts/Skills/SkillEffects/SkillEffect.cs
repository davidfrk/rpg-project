using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills {
    public class SkillEffect : ScriptableObject
    {
        public virtual void Cast(OnSkillEvent skillEvent, Skill skill)
        {

        }
    }
}