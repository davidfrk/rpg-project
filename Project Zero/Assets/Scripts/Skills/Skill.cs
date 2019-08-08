using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class Skill : MonoBehaviour
    {
        public SkillType skillType;
        public float castRange = 3f;
        public float manaCost = 50;
        public bool canCastOnGround = true;
        public bool canCastOnUnit = true;
        public Sprite icon;
        [SerializeField]
        private bool canBeInterrupted = false;
        private Unit owner;
        public Unit Owner
        {
            get
            {
                return owner;
            }
            set
            {
                if (value != null)
                {
                    RegisterEvents(value);
                }
                else
                {
                    UnRegisterEvents();
                }
                owner = value;
            }
        }
        public string description;

        protected AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void Cast(Unit owner, Transform castTransform, Vector3 targetPosition)
        {

        }

        public virtual void Cast(Unit owner, Transform castTransform, Unit targetUnit)
        {

        }

        public virtual void OnCastEnd()
        {

        }

        public virtual bool CanCast(Unit owner)
        {
            return skillType == SkillType.Active && owner.Mana >= manaCost;
        }

        public virtual bool CanBeInterrupted()
        {
            return canBeInterrupted;
        }

        public virtual void Interrupt()
        {

        }

        protected virtual void RegisterEvents(Unit owner)
        {
            
        }

        protected virtual void UnRegisterEvents()
        {
            
        }

        public static Collider[] FindUnitsInBox(Vector3 center, Vector3 dimensions, Quaternion orientation)
        {
            return Physics.OverlapBox(center, dimensions, orientation, LayerMask.GetMask("Unit"));
        }

        public enum SkillType
        {
            Passive,
            Active
        }

        public enum BonusType
        {
            Flat,
            Mult,
        }
    }
}
