using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills.SkillEvent {
    public class OnSkillEvent : MonoBehaviour
    {
        protected Skill skill;
        private Unit owner;
        public Unit Owner
        {
            get
            {
                return owner;
            }
            set
            {
                if (owner != null)
                {
                    UnRegisterEvents(owner);
                }

                if (value != null)
                {
                    RegisterEvents(value);
                }

                owner = value;
            }
        }

        public List<SkillEffect> skillEffects;

        internal Unit target;
        internal float damage;

        void Awake()
        {
            skill = GetComponent<Skill>();
            if (skill == null) Debug.Log(gameObject.name);
            else skill.OnOwnerChangeCallback += OnSkillOwnerChange;
        }

        private void OnSkillOwnerChange(Unit owner)
        {
            Owner = owner;
        }

        protected virtual void RegisterEvents(Unit owner)
        {
            
        }

        protected virtual void UnRegisterEvents(Unit owner)
        {
            
        }

        protected virtual void Cast()
        {
            foreach (SkillEffect skillEffect in skillEffects)
            {
                skillEffect.Cast(this, skill);
            }
        }
    }
}