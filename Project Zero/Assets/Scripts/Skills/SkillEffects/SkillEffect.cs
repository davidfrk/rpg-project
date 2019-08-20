using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills {
    public class SkillEffect : ScriptableObject
    {
        public SkillEventType eventType;

        public void Init(Skill skill)
        {
            RegisterEvents(skill);
        }

        private void RegisterEvents(Skill skill)
        {
            switch (eventType)
            {
                case SkillEventType.OnCast:
                    {
                        skill.OnCastCallback += Cast;
                        break;
                    }
                case SkillEventType.OnCastStart:
                    {
                        skill.OnCastStartCallback += Cast;
                        break;
                    }
                case SkillEventType.OnAction:
                    {
                        skill.OnActionCallback += Cast;
                        break;
                    }
                case SkillEventType.OnCastEnd:
                    {
                        skill.OnCastEndCallback += Cast;
                        break;
                    }
                case SkillEventType.OnAttack:
                    {
                        skill.OnActionCallback += Cast;
                        break;
                    }
                case SkillEventType.OnKill:
                    {
                        skill.OnActionCallback += Cast;
                        break;
                    }
            }
        }

        protected virtual void Cast(Skill skill)
        {

        }
    }
}