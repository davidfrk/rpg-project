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

    public float movementSpeed = 1f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
        movementController.OnStateChangeCallback += OnStateUpdate;
        unitController = GetComponent<UnitController>();
        unitController.OnCastBeginCallback += OnCastBegin;
        unitController.OnStateChangeCallback += OnStateUpdate;
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
        animator.SetFloat("AttackSpeed", unitController.unit.AttackSpeed);
        animator.SetFloat("MovementSpeed", movementSpeed * unitController.unit.MovementSpeed);
    }

    void OnStateUpdate(UnitState state)
    {
        animator.SetBool("Running", movementController.MovementState == MovementState.Running);
        animator.SetBool("Attacking", state == UnitState.Attacking);
        animator.SetBool("Casting", state == UnitState.Casting);
        animator.SetBool("Dead", state == UnitState.Dead);
    }
    
    void OnStateUpdate(MovementState state)
    {
        animator.SetBool("Running", state == MovementState.Running);
        animator.SetBool("Attacking", unitController.State == UnitState.Attacking);
        animator.SetBool("Casting", unitController.State == UnitState.Casting);
        animator.SetBool("Dead", unitController.State == UnitState.Dead);
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
