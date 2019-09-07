using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class Skill : MonoBehaviour
    {
        public SkillType skillType;
        public List<SkillTag> skillTags;

        public float castRange = 3f;
        public float manaCost = 50;
        public bool forceSelfCast = false;
        public bool canSelfCast = true;
        public bool canCastOnUnit = true;
        public bool canCastOnGround = true;
        
        public Sprite icon;
        public string key;
        new public AnimationClip animation;

        [SerializeField]
        private bool canBeInterrupted = false;
        public string description;

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

                OnOwnerChangeCallback?.Invoke(owner);
            }
        }
        public Unit TargetUnit { get; private set; }
        public Vector3 TargetPosition { get; private set; }
        
        public AudioSource AudioSource { get; private set; }

        public delegate void OwnerChangeEvent(Unit Owner);
        public event OwnerChangeEvent OnOwnerChangeCallback;

        public delegate void CastEvent();
        public event CastEvent OnCastCallback;

        public delegate void CastStartEvent();
        public event CastStartEvent OnCastStartCallback;

        public delegate void ActionEvent();
        public event ActionEvent OnActionCallback;

        public delegate void CastEndEvent();
        public event CastEndEvent OnCastEndCallback;

        void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        public virtual UnitState OnCast()
        {
            OnCastCallback?.Invoke();
            return UnitState.Casting;
        }

        public virtual void OnCastStart(Unit owner, Transform castTransform, Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
            OnCastStartCallback?.Invoke();
        }

        public virtual void OnCastStart(Unit owner, Transform castTransform, Unit targetUnit)
        {
            TargetUnit = targetUnit;
            OnCastStartCallback?.Invoke();
        }

        public virtual void OnAction()
        {
            OnActionCallback?.Invoke();
        }

        public virtual void OnCastEnd()
        {
            OnCastEndCallback?.Invoke();
        }

        public virtual bool CanCast(Unit owner)
        {
            return skillType == SkillType.Active && owner.Mana >= manaCost;
        }

        public virtual bool CanCastOnTarget(Unit target)
        {
            if (!canCastOnUnit)
            {
                return false;
            }

            if (!canSelfCast && target == Owner)
            {
                return false;
            }

            return true;
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

        protected virtual void UnRegisterEvents(Unit owner)
        {
            
        }

        public static Collider[] FindUnitsInBox(Vector3 center, Vector3 dimensions, Quaternion orientation)
        {
            return Physics.OverlapBox(center, dimensions, orientation, LayerMask.GetMask("Unit"));
        }

        public static Collider[] FindUnitsInSphere(Vector3 center, float radius)
        {
            return Physics.OverlapSphere(center, radius, LayerMask.GetMask("Unit"));
        }

        public enum SkillType
        {
            Passive,
            Active
        }

        public enum EffectType
        {
            Positive,
            Negative,
        }
    }
}
