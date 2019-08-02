using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rpg.Items;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AnimatorUpdate))]
[RequireComponent(typeof(Unit))]
public class UnitController : MonoBehaviour
{
    public bool playerUnit = false;
    public PlayerController owner;

    [SerializeField]
    private UnitState state = UnitState.Idle;
    public UnitState State
    {
        get
        {
            return state;
        }
        set
        {
            if (value != state)
            {
                state = value;
                OnStateChangeCallback?.Invoke(state);
            }
        }
    }

    internal Unit unit;
    MovementController movementController;
    NavMeshAgent navMeshAgent;
    EquipmentManager equipmentManager;
    internal SkillsManager castController;

    //[SerializeField]
    public Action action = new Action();
    Vector3 targetPosition;
    Unit targetUnit;

    public delegate void StateChangeEvent(UnitState state);
    public event StateChangeEvent OnStateChangeCallback;

    public delegate void OnBeingAttackedEvent(Unit aggressor);
    public event OnBeingAttackedEvent OnBeingAttackedCallback;

    public delegate void DeathEvent(Unit killer);
    public event DeathEvent OnDeathCallback;

    public delegate void KillEvent(Unit prey);
    public event KillEvent OnKillCallback;

    void Awake()
    {
        unit = GetComponent<Unit>();
        movementController = GetComponent<MovementController>();
        movementController.OnMovementCallback += MovementCallback;
        navMeshAgent = GetComponent<NavMeshAgent>();
        equipmentManager = GetComponent<EquipmentManager>();
        castController = GetComponent<SkillsManager>();
    }

    void Update()
    {
        if (State == UnitState.MovingToAct)
        {
            MovementUpdate();
        }
    }

    void MovementCallback()
    {
        //Debug.Log(gameObject + " MovementCallback " + State + " action " + action.actionType);
        if (State == UnitState.Moving)
        {
            State = UnitState.Idle;
        }

        if (State == UnitState.MovingToAct)
        {
            switch (action.actionType)
            {
                case Action.ActionType.Attack:
                    {
                        Attack(targetUnit);
                        break;
                    }
                case Action.ActionType.PickItem:
                    {
                        PickItem(action.item);
                        break;
                    }
                case Action.ActionType.Cast:
                    {
                        Cast();
                        break;
                    }
            }
        }
    }

    public void MoveAttack(Unit target)
    {
        if (!StopCurrentAction()) return;

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Attack;
        action.targetUnit = target;

        targetUnit = target;
        movementController.MoveCloseToPosition(target.transform.position, DistanceToAttack());
    }

    private void MovementUpdate()
    {
        if (action.actionType == Action.ActionType.Attack)
        {
            if (targetUnit != null && targetUnit.alive)
            {
                movementController.MoveCloseToPosition(targetUnit.transform.position, DistanceToAttack());
            }
            else
            {
                State = UnitState.Idle;
            }
        }
        else if (action.actionType == Action.ActionType.Cast && action.targetUnit != null)
        {
            if (action.targetUnit.alive)
            {
                movementController.MoveCloseToPosition(action.targetUnit.transform.position, action.skill.castRange);
            }
            else
            {
                State = UnitState.Idle;
            }
        }
    }

    float DistanceToAttack()
    {
        return unit.radius + targetUnit.radius + 0.2f;
    }

    public void Move(Vector3 position)
    {
        if (!StopCurrentAction()) return;

        State = UnitState.Moving;
        targetPosition = position;
        movementController.MoveToPosition(targetPosition);

        //Debug.Log("MoveCmd");
    }

    void Attack(Unit target)
    {
        State = UnitState.Attacking;
        targetUnit = target;
    }

    void Cast()
    {
        State = UnitState.Casting;
    }

    bool StopCurrentAction()
    {
        if (State == UnitState.Casting)
        {
            if (action.skill.CanBeInterrupted())
            {
                action.skill.Interrupt();
                State = UnitState.Idle;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public void AttackEndEvent()
    {
        if (targetUnit != null)
        {
            if (unit.alive && State == UnitState.Attacking)
            {
                targetUnit.TakeDamage(unit.stats.Attack.Value, DamageType.Physical, this.unit);
                targetUnit.unitController.OnBeingAttackedEventCallback(this.unit);

                if (targetUnit.alive)
                {
                    Vector3 targetDir = targetUnit.transform.position - transform.position;
                    if (targetDir.magnitude > DistanceToAttack() || Vector3.Angle(transform.forward, targetDir) > 5f)
                    {
                        MoveAttack(targetUnit);
                    }
                }
                else
                {
                    State = UnitState.Idle;
                    targetUnit = null;
                }
            }
        }
    }

    public void OnCastEnd()
    {
        if (State == UnitState.Casting)
        {
            State = UnitState.Idle;
        }
    }

    private void OnBeingAttackedEventCallback(Unit aggressor)
    {
        OnBeingAttackedCallback(aggressor);
    }

    public void Die(Unit killer)
    {
        State = UnitState.Dead;
        navMeshAgent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Corpse");

        OnDeathCallback?.Invoke(killer);

        if (killer != null)
        {
            UnitController killerController = killer.GetComponent<UnitController>();
            killerController.OnKillEvent(this.unit);
        }
    }

    private void OnKillEvent(Unit prey)
    {
        OnKillCallback?.Invoke(prey);

        if (owner != null)
        {
            owner.OnKill(prey);
        }
    }

    public void Spawn(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        unit.Spawn();
        navMeshAgent.enabled = true;
        State = UnitState.Idle;
        gameObject.layer = LayerMask.NameToLayer("Unit");
    }

    public void MoveToPickItem(Item item)
    {
        if (!StopCurrentAction()) return;

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.PickItem;
        action.item = item;

        movementController.MoveCloseToPosition(item.transform.position, 3f * unit.radius);
        //Debug.Log(gameObject + " MoveToPickCmd " + State);
    }

    private void PickItem(Item item)
    {
        if (equipmentManager != null)
        {
            equipmentManager.PickItem(item);
        }
        State = UnitState.Idle;
        //Debug.Log(gameObject +  " PickItem " + item);
    }

    public void MoveToCast(Vector3 position, int skillSlot)
    {
        if (!StopCurrentAction()) return;

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Cast;
        action.skill = castController.skills[skillSlot];
        action.targetUnit = null;

        movementController.MoveCloseToPosition(position, action.skill.castRange);

        //Debug.Log("CastOnGround");
    }

    public void MoveToCast(Unit target, int skillSlot)
    {
        if (!StopCurrentAction()) return;

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Cast;
        action.skill = castController.skills[skillSlot];
        action.targetUnit = target;

        movementController.MoveCloseToPosition(target.transform.position, action.skill.castRange);

        //Debug.Log("CastOnTarget");
    }

    //Workaround para evitar que o agente continue tentando chegar em um posição bloqueada por outro agente
    private void OnTriggerStay(Collider other)
    {
        if (State == UnitState.Moving)
        {
            UnitController unitController = other.GetComponent<UnitController>();
            if (unitController != null && unitController.unit.alive)
            {
                Vector3 targetDir = targetPosition - unitController.transform.position;
                float minDist = unit.radius + unitController.unit.radius + 0.1f;
                if (targetDir.magnitude < minDist)
                {
                    Move(unitController.transform.position + minDist * targetDir.normalized);
                    //movementController.Stop();
                }
            }
        }
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