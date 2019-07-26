using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AnimatorUpdate))]
[RequireComponent(typeof(Unit))]
public class UnitController : MonoBehaviour
{
    public UnitState state = UnitState.Idle;

    internal Unit unit;
    MovementController movementController;
    NavMeshAgent navMeshAgent;
    EquipmentManager equipmentManager;

    [SerializeField]
    Action action = new Action();
    Vector3 targetPosition;
    Unit targetUnit;

    public delegate void OnBeingAttackedEvent(Unit aggressor);
    public event OnBeingAttackedEvent OnBeingAttackedCallback;

    public delegate void DeathEvent(Unit killer);
    public event DeathEvent OnDeathCallback;

    void Awake()
    {
        unit = GetComponent<Unit>();
        movementController = GetComponent<MovementController>();
        movementController.OnMovementCallback += MovementCallback;
        navMeshAgent = GetComponent<NavMeshAgent>();
        equipmentManager = GetComponent<EquipmentManager>();
    }

    void Update()
    {
        if (state == UnitState.MovingToAct && action.actionType == Action.ActionType.Attack)
        {
            MoveToAttackUpdate();
        }
    }

    void MovementCallback()
    {
        if (state == UnitState.Moving)
        {
            state = UnitState.Idle;
        }

        if (state == UnitState.MovingToAct)
        {
            if (action.actionType == Action.ActionType.Attack)
            {
                Attack(targetUnit);
            }

            if (action.actionType == Action.ActionType.PickItem)
            {
                PickItem(action.item);
            }
        }
    }

    public void MoveAttack(Unit target)
    {
        state = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Attack;
        action.targetUnit = target;

        targetUnit = target;
        movementController.MoveCloseToPosition(target.transform.position, DistanceToAttack());
    }

    private void MoveToAttackUpdate()
    {
        if (targetUnit == null || !targetUnit.alive)
        {
            state = UnitState.Idle;
        }
        else
        {
            movementController.MoveCloseToPosition(targetUnit.transform.position, DistanceToAttack());
        }
    }

    float DistanceToAttack()
    {
        return unit.radius + targetUnit.radius + 0.2f;
    }

    public void Move(Vector3 position)
    {
        state = UnitState.Moving;
        targetPosition = position;
        movementController.MoveToPosition(targetPosition);
    }

    void Attack(Unit target)
    {
        state = UnitState.Attacking;
        targetUnit = target;
    }

    public void AttackEndEvent()
    {
        if (targetUnit != null)
        {
            targetUnit.TakeDamage(unit.unitStats.Attack, DamageType.Physical, this.unit);
            targetUnit.unitController.OnBeingAttackedEventCallback(this.unit);

            if (unit.alive && state == UnitState.Attacking)
            {
                if (targetUnit.alive)
                {
                    MoveAttack(targetUnit);
                }
                else
                {
                    state = UnitState.Idle;
                    targetUnit = null;
                }
            }
        }
    }

    private void OnBeingAttackedEventCallback(Unit aggressor)
    {
        OnBeingAttackedCallback(aggressor);
    }

    public void Die(Unit killer)
    {
        state = UnitState.Dead;
        navMeshAgent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Corpse");

        OnDeathCallback?.Invoke(killer);
    }

    public void Spawn(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        unit.Spawn();
        navMeshAgent.enabled = true;
        state = UnitState.Idle;
        gameObject.layer = LayerMask.NameToLayer("Unit");
    }

    public void MoveToPickItem(Item item)
    {
        state = UnitState.MovingToAct;
        action.actionType = Action.ActionType.PickItem;
        action.item = item;

        movementController.MoveCloseToPosition(item.transform.position, 3f * unit.radius);
    }

    private void PickItem(Item item)
    {
        if (equipmentManager != null)
        {
            equipmentManager.PickItem(item);
        }
        state = UnitState.Idle;
    }
}

public enum UnitState
{
    Idle,
    Moving,
    MovingToAct,
    Attacking,
    Casting,
    Dead
}