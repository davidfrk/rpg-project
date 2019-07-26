using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class AI : MonoBehaviour
{
    Unit unit;
    UnitController unitController;

    public AggressionType aggression = AggressionType.Neutral;
    public float visionRange = 3f;
    public float broadcastRange = 5f;

    LayerMask unitsLayer;

    void Awake()
    {
        unit = GetComponent<Unit>();
        unitController = GetComponent<UnitController>();
        unitController.OnBeingAttackedCallback += OnBeingAttacked;

        unitsLayer = LayerMask.GetMask("Unit");
    }

    void OnBeingAttacked(Unit aggressor)
    {
        if (unitController.state == UnitState.Idle)
        {
            unitController.MoveAttack(aggressor);
            //Call for assistance
            BroadcastOnBeingAttackedEvent(unit, aggressor);
        }
    }

    private void BroadcastOnBeingAttackedEvent(Unit target, Unit aggressor)
    {
        Collider[] unitsInRange = Physics.OverlapSphere(transform.position, broadcastRange, unitsLayer);
        foreach (Collider unitCollider in unitsInRange)
        {
            AI unitAI = unitCollider.GetComponent<AI>();
            
            //Dont broadcast to yourself
            if (target != unitAI.unit)
            {
                unitAI.ListenOnBeingAttackedEvent(target, aggressor);
            }
        }
    }

    private void ListenOnBeingAttackedEvent(Unit target, Unit aggressor)
    {
        if (unitController.state == UnitState.Idle || unitController.state == UnitState.Moving)
        {
            //Dont attack yourself
            if (aggressor != this.unit)
            {
                unitController.MoveAttack(aggressor);
            }
        }
    }

    void Update()
    {
        if (aggression == AggressionType.Agressive && unitController.state == UnitState.Idle)
        {
            SearchForEnemy();
        }
    }

    void SearchForEnemy()
    {
        Collider[] unitsInRange = Physics.OverlapSphere(transform.position, visionRange, unitsLayer);
        foreach (Collider unitCollider in unitsInRange)
        {
            Unit targetUnit = unitCollider.GetComponent<Unit>();

            //Dont attack yourself
            if (this.unit != targetUnit && targetUnit.alive)
            {
                unitController.MoveAttack(targetUnit);
            }
        }
    }

    public enum AggressionType
    {
        Neutral,
        Agressive
    }
}
