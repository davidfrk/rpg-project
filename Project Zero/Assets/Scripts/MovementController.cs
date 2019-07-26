using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementController : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 targetPosition;
    bool ensureRotation = false;

    [SerializeField]
    MovementState movementState = MovementState.Idle;
    public MovementState MovementState
    {
        get
        {
            return movementState;
        }
    }

    //Callback quando esta em range ou chegou no destino
    public delegate void MovementEvent();
    public event MovementEvent OnMovementCallback;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.enabled)
        {
            StateUpdate();
        }
        else
        {
            movementState = MovementState.Idle;
        }
    }

    void StateUpdate()
    {
        if (movementState != MovementState.Idle) {
            if (!agent.pathPending)
            {
                if (agent.velocity.sqrMagnitude == 0f && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (ensureRotation)
                    {
                        bool isFacingTheTarget = FaceTargetUpdate();
                        if (isFacingTheTarget)
                        {
                            ensureRotation = false;
                            movementState = MovementState.Idle;
                            OnMovementCallback();
                        }
                    }
                    else
                    {
                        movementState = MovementState.Idle;
                        OnMovementCallback();
                    }
                }
            }
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.stoppingDistance = 0f;
        targetPosition = position;
        agent.SetDestination(position);
        movementState = MovementState.Running;
        ensureRotation = false;
    }

    public void MoveCloseToPosition(Vector3 position, float distance)
    {
        agent.stoppingDistance = distance;
        targetPosition = position;
        agent.SetDestination(position);
        movementState = MovementState.Running;
        ensureRotation = true;
    }

    public bool FaceTargetUpdate()
    {
        Vector3 targetDir = targetPosition - transform.position;

        if (targetDir != Vector3.zero)
        {
            Quaternion targetOrientation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, agent.angularSpeed * Time.deltaTime);

            return Quaternion.Angle(transform.rotation, targetOrientation) < 5f;
        }
        else
        {
            return true;
        }
    }
}

public enum MovementState
{
    Idle,
    Walking,
    Running,
}
