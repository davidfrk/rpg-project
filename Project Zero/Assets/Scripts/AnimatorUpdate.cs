using System.Collections;
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
        unitController = GetComponent<UnitController>();
    }

    void Update()
    {
        animator.SetBool("Running", movementController.MovementState == MovementState.Running);
        animator.SetBool("Attacking", unitController.state == UnitState.Attacking);
        animator.SetBool("Dead", unitController.state == UnitState.Dead);
    }
}
