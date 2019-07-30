﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUpdate : MonoBehaviour
{
    Animator animator;
    MovementController movementController;
    UnitController unitController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
        movementController.OnStateChangeCallback += OnStateUpdate;
        unitController = GetComponent<UnitController>();
        unitController.OnStateChangeCallback += OnStateUpdate;
    }

    void Start()
    {
        OnStateUpdate(unitController.State);
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
}
