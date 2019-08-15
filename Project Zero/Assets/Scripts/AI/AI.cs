using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.AI
{
    [RequireComponent(typeof(UnitController))]
    public class AI : MonoBehaviour
    {
        Unit unit;
        UnitController unitController;

        public Faction faction;
        public AggressionType aggression = AggressionType.Neutral;
        public float visionRange = 3f;
        public float broadcastRange = 5f;

        private float attentionTime = 4f;
        private float focusStartTime = 0f;

        LayerMask unitsLayer;
        private float nextSearchForEnemyTime = 0f;

        void Awake()
        {
            unit = GetComponent<Unit>();
            unitController = GetComponent<UnitController>();
            unitController.OnBeingAttackedCallback += OnBeingAttacked;
            unitController.OnAttackEndCallback += OnAttackEnd;
            unitController.OnKillCallback += OnKill;

            unitsLayer = LayerMask.GetMask("Unit");
        }

        void OnBeingAttacked(Unit aggressor)
        {
            if (aggressor.alive)
            {
                if (unitController.State == UnitState.Idle)
                {
                    Attack(aggressor);
                }

                //Call for assistance, 
                BroadcastOnBeingAttackedEvent(unit, aggressor);
            }
        }

        private void BroadcastOnBeingAttackedEvent(Unit target, Unit aggressor)
        {
            //ToDo: limit the interval between broadcasts
            Collider[] unitsInRange = Physics.OverlapSphere(transform.position, broadcastRange, unitsLayer);
            foreach (Collider unitCollider in unitsInRange)
            {
                AI allyUnit = unitCollider.GetComponent<AI>();

                if (faction == allyUnit.faction)
                {
                    //Dont broadcast to yourself
                    if (target != allyUnit.unit)
                    {
                        allyUnit.ListenOnBeingAttackedEvent(target, aggressor);
                    }
                }
            }
        }

        private void ListenOnBeingAttackedEvent(Unit target, Unit aggressor)
        {
            if (unitController.State == UnitState.Idle || unitController.State == UnitState.Moving)
            {
                if (!unitController.playerUnit && faction == target.GetComponent<AI>().faction)
                {
                    //Dont attack yourself
                    if (aggressor != this.unit)
                    {
                        Attack(aggressor);
                    }
                }
            }
        }

        void Attack(Unit target)
        {
            if (target != null)
            {
                unitController.MoveAttack(target);
                FocusOnEnemy(target);
            }
        }

        void OnAttackEnd(Unit target, float damage)
        {
            FocusOnEnemy(target);
        }

        void OnKill(Unit killer)
        {
            AI targetEnemy = SearchForEnemy();
            if (targetEnemy)
            {
                Attack(targetEnemy.unit);
            }
            else
            {
                unitController.Stop();
            }
        }

        void FocusOnEnemy(Unit target)
        {
            focusStartTime = Time.time;
        }

        void LoseFocus()
        {
            unitController.Stop();
        }

        void Update()
        {
            if (aggression == AggressionType.Agressive && unitController.State == UnitState.Idle)
            {
                if (Time.time >= nextSearchForEnemyTime)
                {
                    AI targetEnemy = SearchForEnemy();
                    if (targetEnemy) Attack(targetEnemy.unit);
                }
            }

            if (!unitController.playerUnit && unitController.State == UnitState.MovingToAct && Time.time > (focusStartTime + attentionTime))
            {
                LoseFocus();
            }
        }

        AI SearchForEnemy()
        {
            AI nearestEnemy = null;
            float nearestDistance = Mathf.Infinity;

            Collider[] unitsInRange = Physics.OverlapSphere(transform.position, visionRange, unitsLayer);
            foreach (Collider unitCollider in unitsInRange)
            {
                AI targetUnit = unitCollider.GetComponent<AI>();

                if (faction != targetUnit.faction && targetUnit.unit.alive)
                {
                    //Dont attack yourself
                    if (this != targetUnit)
                    {
                        float sqrDistance = (targetUnit.transform.position - transform.position).sqrMagnitude;
                        if (sqrDistance < nearestDistance)
                        {
                            nearestEnemy = targetUnit;
                            nearestDistance = sqrDistance;
                        }
                    }
                }
            }
            nextSearchForEnemyTime = Time.time + 0.3f + 0.3f * Random.value;

            return nearestEnemy;
        }

        public enum AggressionType
        {
            Neutral,
            Agressive
        }
    }
}
