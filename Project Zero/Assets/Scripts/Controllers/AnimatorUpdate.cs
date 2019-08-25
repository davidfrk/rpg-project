using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUpdate : MonoBehaviour
{
    Animator animator;
    MovementController movementController;
    UnitController unitController;
    public bool allowOverride = false;
    public AnimatorOverrideController baseAnimatorOverrideController;
    protected AnimatorOverrideController animatorOverrideController;
    protected AnimationClipOverrides clipOverrides;

    public float attack1Speed = 1f;
    public float attack2Speed = 1f;
    public float movementSpeed = 1f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
        movementController.OnStateChangeCallback += OnStateUpdate;
        unitController = GetComponent<UnitController>();
        unitController.OnCastBeginCallback += OnCastBegin;
        unitController.OnStateChangeCallback += OnStateUpdate;
        unitController.OnAttackEndCallback += OnAttackEnd;
    }

    void Start()
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(baseAnimatorOverrideController.overridesCount);
        baseAnimatorOverrideController.GetOverrides(clipOverrides);

        animatorOverrideController.ApplyOverrides(clipOverrides);

        OnStateUpdate(unitController.State);
    }

    void Update()
    {
        animator.SetFloat("Attack1Speed", attack1Speed * unitController.unit.AttackSpeed);
        animator.SetFloat("Attack2Speed", attack2Speed * unitController.unit.AttackSpeed);
        animator.SetFloat("MovementSpeed", movementSpeed * unitController.unit.MovementSpeed);
        //animator.SetBool("Crit", unitController.IsCriticalAttack);
    }

    void OnStateUpdate(UnitState state)
    {
        UpdateState();
    }
    
    void OnStateUpdate(MovementState state)
    {
        UpdateState();
    }

    void OnAttackEnd(Unit attacker, float damage)
    {
        animator.SetBool("Crit", unitController.IsCriticalAttack);
    }

    void UpdateState()
    {
        animator.SetBool("Running", movementController.MovementState == MovementState.Running);
        animator.SetBool("Attacking", unitController.State == UnitState.Attacking);
        animator.SetBool("Casting", unitController.State == UnitState.Casting);
        animator.SetBool("Dead", unitController.State == UnitState.Dead);
        animator.SetBool("Crit", unitController.IsCriticalAttack);
    }

    void OnCastBegin()
    {
        if (allowOverride)
        {
            clipOverrides["Cast"] = unitController.action.skill.animation;
            animatorOverrideController.ApplyOverrides(clipOverrides);
        }
    }
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }

    public AnimationClip this[string name]
    {
        get { return this.Find(x => x.Key.name.Equals(name)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}
