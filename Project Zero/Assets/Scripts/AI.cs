using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class AI : MonoBehaviour
{
    Unit unit;
    UnitController unitController;

    public Faction faction;
    public AggressionType aggression = AggressionType.Neutral;
    public float visionRange = 3f;
    public float broadcastRange = 5f;

    LayerMask unitsLayer;
    private float nextSearchForEnemyTime = 0f;

    void Awake()
    {
        unit = GetComponent<Unit>();
        unitController = GetComponent<UnitController>();
        unitController.OnBeingAttackedCallback += OnBeingAttacked;

        unitsLayer = LayerMask.GetMask("Unit");
    }

    void OnBeingAttacked(Unit aggressor)
    {
        if (aggressor.alive)
        {
            if (unitController.State == UnitState.Idle)
            {
                unitController.MoveAttack(aggressor);
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
                    unitController.MoveAttack(aggressor);
                }
            }
        }
    }

    void Update()
    {
        if (aggression == AggressionType.Agressive && unitController.State == UnitState.Idle)
        {
            if (Time.time >= nextSearchForEnemyTime)
            {
                SearchForEnemy();
            }
        }
    }

    void SearchForEnemy()
    {
        Collider[] unitsInRange = Physics.OverlapSphere(transform.position, visionRange, unitsLayer);
        foreach (Collider unitCollider in unitsInRange)
        {
            AI targetUnit = unitCollider.GetComponent<AI>();

            if (faction != targetUnit.faction && targetUnit.unit.alive)
            {
                //Dont attack yourself
                if (this != targetUnit)
                {
                    unitController.MoveAttack(targetUnit.unit);
                }
            }
        }
        nextSearchForEnemyTime = Time.time + 0.3f + 0.3f * Random.value;
    }

    public enum AggressionType
    {
        Neutral,
        Agressive
    }

    public enum Faction
    {
        Human,
        Monster,
        AnimalPredator,
        Animal,
    }
}
