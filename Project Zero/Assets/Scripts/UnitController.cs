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

                if (state == UnitState.Idle)
                {
                    action.actionType = Action.ActionType.None;
                }
            }
        }
    }

    internal Unit unit;
    MovementController movementController;
    NavMeshAgent navMeshAgent;
    internal EquipmentManager equipmentManager;
    internal SkillsManager castController;

    //[SerializeField]
    public Action action;
    private Action actionQueue = new Action();

    public delegate void StateChangeEvent(UnitState state);
    public event StateChangeEvent OnStateChangeCallback;

    public delegate void OnBeingAttackedEvent(Unit aggressor);
    public event OnBeingAttackedEvent OnBeingAttackedCallback;

    public delegate void OnAttackEndEvent(Unit target, float damage);
    public event OnAttackEndEvent OnAttackEndCallback;

    public delegate void SpawnEvent(Unit unit);
    public event SpawnEvent OnSpawnCallback;

    public delegate void DeathEvent(Unit killer);
    public event DeathEvent OnDeathCallback;

    public delegate void KillEvent(Unit prey);
    public event KillEvent OnKillCallback;

    public delegate void CastBeginEvent();
    public event CastBeginEvent OnCastBeginCallback;

    void Awake()
    {
        action.actionType = Action.ActionType.None;
        actionQueue.actionType = Action.ActionType.None;
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

        if (State == UnitState.Idle)
        {
            ResumeAction();
        }
    }

    void MovementCallback()
    {
        //Debug.Log(gameObject + " MovementCallback ");
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
                        Attack(action.targetUnit);
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
                default: break;
            }
        }
    }

    public void MoveAttack(Unit target)
    {
        if (!StopCurrentAction())
        {
            actionQueue.actionType = Action.ActionType.Attack;
            actionQueue.targetUnit = target;
            return;
        }

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Attack;
        action.targetUnit = target;


        movementController.MoveCloseToPosition(target.transform.position, DistanceToAttack());
    }

    private void MovementUpdate()
    {
        if (action.actionType == Action.ActionType.Attack)
        {
            if (action.targetUnit != null && action.targetUnit.alive)
            {
                movementController.MoveCloseToPosition(action.targetUnit.transform.position, DistanceToAttack());
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
        }else if (action.actionType == Action.ActionType.PickItem)
        {
            if (action.item.State == Item.ItemState.InWorld)
            {
                movementController.MoveCloseToPosition(action.item.transform.position, DistanceToPickItem());
            }
            else
            {
                State = UnitState.Idle;
            }
        }else if (action.actionType == Action.ActionType.Move)
        {
            movementController.MoveToPosition(action.targetPosition);
            State = UnitState.Moving;
        }
    }

    float DistanceToAttack()
    {
        return unit.radius + action.targetUnit.radius + 0.2f;
    }

    float DistanceToPickItem()
    {
        return 3f * unit.radius;
    }

    public void Move(Vector3 position)
    {
        if (!StopCurrentAction())
        {
            actionQueue.actionType = Action.ActionType.Move;
            actionQueue.targetPosition = position;
            return;
        }

        State = UnitState.Moving;
        action.targetPosition = position;
        movementController.MoveToPosition(action.targetPosition);

        //Debug.Log("MoveCmd");
    }

    void Attack(Unit target)
    {
        State = UnitState.Attacking;
        //targetUnit = target;
    }
    
    void Cast()
    {
        OnCastBeginCallback?.Invoke();
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
        if (action.targetUnit != null)
        {
            if (unit.alive && State == UnitState.Attacking)
            {
                float attackDamage = unit.stats.Attack.Value;
                action.targetUnit.TakeDamage(attackDamage, DamageType.Physical, this.unit);
                OnAttackEndCallback?.Invoke(action.targetUnit, attackDamage);

                if (action.targetUnit.alive)
                {
                    Vector3 targetDir = action.targetUnit.transform.position - transform.position;
                    if (targetDir.magnitude > DistanceToAttack() || Vector3.Angle(transform.forward, targetDir) > 5f)
                    {
                        MoveAttack(action.targetUnit);
                    }
                }
                else
                {
                    State = UnitState.Idle;
                    action.actionType = Action.ActionType.None;
                    action.targetUnit = null;
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

    public void CallOnBeingAttackedEvent(Unit aggressor)
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
        OnSpawnCallback?.Invoke(this.unit);
    }

    public void MoveToPickItem(Item item)
    {
        if (!StopCurrentAction())
        {
            actionQueue.actionType = Action.ActionType.PickItem;
            actionQueue.item = item;
            return;
        }

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.PickItem;
        action.item = item;

        movementController.MoveCloseToPosition(item.transform.position, DistanceToPickItem());
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
        if (!StopCurrentAction())
        {
            actionQueue.actionType = Action.ActionType.Cast;
            actionQueue.skill = castController.skills[skillSlot];
            actionQueue.targetUnit = null;
            actionQueue.targetPosition = position;
            return;
        }

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Cast;
        action.skill = castController.skills[skillSlot];
        action.targetUnit = null;

        movementController.MoveCloseToPosition(position, action.skill.castRange);

        //Debug.Log("CastOnGround");
    }

    public void MoveToCast(Unit target, int skillSlot)
    {
        if (!StopCurrentAction())
        {
            actionQueue.actionType = Action.ActionType.Cast;
            actionQueue.skill = castController.skills[skillSlot];
            actionQueue.targetUnit = target;
            return;
        }

        State = UnitState.MovingToAct;
        action.actionType = Action.ActionType.Cast;
        action.skill = castController.skills[skillSlot];
        action.targetUnit = target;

        movementController.MoveCloseToPosition(target.transform.position, action.skill.castRange);

        //Debug.Log("CastOnTarget");
    }

    private void ResumeAction()
    {
        if (actionQueue.actionType != Action.ActionType.None)
        {
            action = actionQueue;
            actionQueue.actionType = Action.ActionType.None;
            State = UnitState.MovingToAct;
        }
    }

    //Workaround para evitar que o agente continue tentando chegar em um posição bloqueada por outro agente
    private void OnTriggerStay(Collider other)
    {
        if (State == UnitState.Moving)
        {
            UnitController unitController = other.GetComponent<UnitController>();
            if (unitController != null && unitController.unit.alive)
            {
                Vector3 targetDir = action.targetPosition - unitController.transform.position;
                float minDist = unit.radius + unitController.unit.radius + 0.1f;
                if (targetDir.magnitude < minDist)
                {
                    Move(unitController.transform.position + minDist * targetDir.normalized);
                    //movementController.Stop();
                }
            }
        }
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        navMeshAgent.enabled = false;
        transform.position = position;
        transform.rotation = rotation;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(position);
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