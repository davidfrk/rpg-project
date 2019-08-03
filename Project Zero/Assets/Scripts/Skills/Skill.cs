using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Skills
{
    public class Skill : MonoBehaviour
    {
        public float castRange = 3f;
        public float manaCost = 50;
        public bool canCastOnGround = true;
        public bool canCastOnUnit = true;
        public Sprite icon;
        [SerializeField]
        private bool canBeInterrupted = false;
        internal Unit owner;
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
            return owner.Mana >= manaCost;
        }

        public virtual bool CanBeInterrupted()
        {
            return canBeInterrupted;
        }

        public virtual void Interrupt()
        {

        }

        public static Collider[] FindUnitsInBox(Vector3 center, Vector3 dimensions, Quaternion orientation)
        {
            return Physics.OverlapBox(center, dimensions, orientation, LayerMask.GetMask("Unit"));
        }
    }
}
