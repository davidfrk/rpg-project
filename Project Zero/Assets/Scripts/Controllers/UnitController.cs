using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rpg.Items;
using Rpg.Stats;
using Rpg.Skills;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AnimatorUpdate))]
[RequireComponent(typeof(Unit))]
public class UnitController : MonoBehaviour
{
    public bool isPlayerUnit = false;
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
            if (value != state && value != UnitState.None)
            {
                if (State == UnitState.Casting)
                {
                    action.skill.Interrupt();
                }

                if (unit.alive || value == UnitState.Dead)
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
    }

    internal Unit unit;
    internal  MovementController movementController;
    NavMeshAgent navMeshAgent;
    internal EquipmentManager equipmentManager;
    internal SkillsManager skillsManager;

    //[SerializeField]
    public Action action;
    private Action actionQueue = new Action();

    public delegate void StateChangeEvent(UnitState state);
    public event StateChangeEvent OnStateChangeCallback;

    public delegate void OnBeingAttackedEvent(Unit aggressor);
    public event OnBeingAttackedEvent OnBeingAttackedCallback;

    public delegate void OnAttackEndEvent(Unit target, float damage);
    public event OnAttackEndEvent OnAttackEndCallback;

    public delegate void CritEvent(float damage);
    public event CritEvent OnCritCallback;

    public delegate void SpawnEvent(Unit unit);
    public event SpawnEvent OnSpawnCallback;

    public delegate void DeathEvent(Unit killer);
    public event DeathEvent OnDeathCallback;

    public delegate void KillEvent(Unit prey);
    public event KillEvent OnKillCallback;

    public delegate void CastBeginEvent();
    public event CastBeginEvent OnCastBeginCallback;

    private Crit crit = null;
    public bool IsCriticalAttack
    {
        get
        {
            return crit != null;
        }
    }

    internal CharacterSoundManager audioManager;

    void Awake()
    {
        action.actionType = Action.ActionType.None;
        actionQueue.actionType = Action.ActionType.None;
        unit = GetComponent<Unit>();
        movementController = GetComponent<MovementController>();
        movementController.OnMovementCallback += MovementCallback;
        navMeshAgent = GetComponent<NavMeshAgent>();
        equipmentManager = GetComponent<EquipmentManager>();
        skillsManager = GetComponent<SkillsManager>();
        audioManager = GetComponent<CharacterSoundManager>();
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

    public void MoveToAttack(Unit target)
    {
        //Prevent canceling attack animation with another attack order to the same target
        if (State == UnitState.Attacking && action.targetUnit == target)
        {
            //Attacking the same target with good conditions
            Vector3 targetDir = target.transform.position - transform.position;
            if (targetDir.magnitude < DistanceToAttack() && AttackAngle(target) <= 5f)
            {
                return;
            }
        }

        //If you cant attack now, put in the queue
        if (!CanStopCurrentAction)
        {
            actionQueue.actionType = Action.ActionType.Attack;
            actionQueue.targetUnit = target;
            return;
        }

        //Set attack order
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
        return 1.05f * (unit.radius + action.targetUnit.radius) + unit.range;
    }

    float AttackAngle(Unit target)
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.y = 0f;

        return Mathf.Abs(Vector3.Angle(transform.forward, targetDir));
    }

    float DistanceToPickItem()
    {
        return 3f * unit.radius;
    }

    public void Stop()
    {
        Move(transform.position);
    }

    public void Move(Vector3 position)
    {
        if (!CanStopCurrentAction)
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
    }
    
    void Cast()
    {
        State = skillsManager.Cast(action);
        if(State == UnitState.Casting) OnCastBeginCallback?.Invoke();
    }

    bool CanStopCurrentAction
    {
        get
        {
            if (State == UnitState.Casting)
            {
                if (action.skill.CanBeInterrupted())
                {
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
    }

    void StopCurrentAction()
    {
        State = UnitState.Idle;
    }

    public void AttackEndEvent()
    {
        if (action.targetUnit != null)
        {
            if (unit.alive && State == UnitState.Attacking)
            {
                //If target is alive and in range attack
                if (action.targetUnit.alive && (action.targetUnit.transform.position - transform.position).magnitude < DistanceToAttack())
                {
                    float attackDamage = unit.stats.Attack.Value;
                    if (crit != null)
                    {
                        attackDamage *= crit.criticalDamage * unit.CritMult;
                        OnCritCallback?.Invoke(attackDamage);
                        HitManager.Crit(unit, action.targetUnit, attackDamage);
                    }
                    action.targetUnit.TakeDamage(attackDamage, DamageType.Physical, this.unit);

                    //Decide next crit, that we can have different animations to attack and crit
                    crit = unit.crit.Proc();

                    OnAttackEndCallback?.Invoke(action.targetUnit, attackDamage);
                }

                //If he still alive move to attack, else Idle
                if (action.targetUnit.alive)
                {
                    MoveToAttack(action.targetUnit);
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

    public void AnimatorOnCastEnd()
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
        if (!CanStopCurrentAction)
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
    }

    public void MoveToCast(Vector3 position, Skill skill)
    {
        if (!CanStopCurrentAction)
        {
            QueueAction(position, skill);
            return;
        }

        SetAction(position, skill);

        if (skill.needToMoveToCast)
        {
            State = UnitState.MovingToAct;
            movementController.MoveCloseToPosition(position, action.skill.castRange);
        }
        else
        {
            Cast();
        }
    }

    public void MoveToCast(Unit target, Skill skill)
    {
        //Avoids canceling animation by casting the same skill twice
        if (State == UnitState.Casting && action.skill == skill && action.targetUnit == target) return;

        if (!CanStopCurrentAction)
        {
            QueueAction(target, skill);
            return;
        }

        SetAction(target, skill);

        if (skill.needToMoveToCast)
        {
            State = UnitState.MovingToAct;
            movementController.MoveCloseToPosition(target.transform.position, action.skill.castRange);
        }
        else
        {
            Cast();
        }
    }

    private void SetAction(Unit target, Skill skill)
    {
        action.actionType = Action.ActionType.Cast;
        action.skill = skill;
        action.targetUnit = target;
        action.targetPosition = target.transform.position;
    }

    private void SetAction(Vector3 position, Skill skill)
    {
        action.actionType = Action.ActionType.Cast;
        action.skill = skill;
        action.targetUnit = null;
        action.targetPosition = position;
    }

    private void QueueAction(Unit target, Skill skill)
    {
        actionQueue.actionType = Action.ActionType.Cast;
        actionQueue.skill = skill;
        actionQueue.targetUnit = target;
        actionQueue.targetPosition = target.transform.position;
    }

    private void QueueAction(Vector3 position, Skill skill)
    {
        actionQueue.actionType = Action.ActionType.Cast;
        actionQueue.skill = skill;
        actionQueue.targetUnit = null;
        actionQueue.targetPosition = position;
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
    Dead,
    None,
}