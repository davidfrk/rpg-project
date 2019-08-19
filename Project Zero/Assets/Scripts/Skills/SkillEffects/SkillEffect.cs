using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills {
    public class SkillEffect : MonoBehaviour
    {
        public SkillEventType eventType;
        protected Skill skill;

        void Awake()
        {
            skill = GetComponent<Skill>();
            RegisterEvents();
        }

        private void RegisterEvents()
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
            }
        }

        protected virtual void Cast()
        {

        }
    }
}