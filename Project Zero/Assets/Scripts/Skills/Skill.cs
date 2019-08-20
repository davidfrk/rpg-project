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
        public bool canCastOnGround = true;
        public bool canCastOnUnit = true;
        public Sprite icon;
        public string key;
        new public AnimationClip animation;

        [SerializeField]
        private bool canBeInterrupted = false;
        public string description;

        public List<SkillEffect> skillEffects;

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
        
        protected AudioSource audioSource;

        public delegate void CastEvent(Skill skill);
        public event CastEvent OnCastCallback;

        public delegate void CastStartEvent(Skill skill);
        public event CastStartEvent OnCastStartCallback;

        public delegate void ActionEvent(Skill skill);
        public event ActionEvent OnActionCallback;

        public delegate void CastEndEvent(Skill skill);
        public event CastEndEvent OnCastEndCallback;

        //Transformar em um struct ou outra estrutura?
        internal Unit target;
        internal float damageOnTarget = 0f;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            foreach (SkillEffect skillEffect in skillEffects)
            {
                skillEffect.Init(this);
            }
        }

        public virtual UnitState OnCast()
        {
            OnCastCallback?.Invoke(this);
            return UnitState.Casting;
        }

        public virtual void OnCastStart(Unit owner, Transform castTransform, Vector3 targetPosition)
        {
            OnCastStartCallback?.Invoke(this);
        }

        public virtual void OnCastStart(Unit owner, Transform castTransform, Unit targetUnit)
        {
            OnCastStartCallback?.Invoke(this);
        }

        public virtual void OnAction()
        {
            OnActionCallback?.Invoke(this);
        }

        public virtual void OnCastEnd()
        {
            OnCastEndCallback?.Invoke(this);
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
