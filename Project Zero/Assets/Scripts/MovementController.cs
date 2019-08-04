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
    private MovementState movementState = MovementState.Idle;
    public MovementState MovementState
    {
        get
        {
            return movementState;
        }
        set
        {
            if (value != movementState)
            {
                movementState = value;
                OnStateChangeCallback?.Invoke(movementState);
            }
        }
    }

    //Callback quando esta em range ou chegou no destino
    public delegate void MovementEvent();
    public event MovementEvent OnMovementCallback;

    public delegate void StateChangeEvent(MovementState state);
    public event StateChangeEvent OnStateChangeCallback;

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
            MovementState = MovementState.Idle;
        }
    }

    void StateUpdate()
    {
        if (MovementState != MovementState.Idle) {
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
                            MovementState = MovementState.Idle;
                            OnMovementCallback();
                        }
                    }
                    else
                    {
                        MovementState = MovementState.Idle;
                        OnMovementCallback();
                    }
                }
            }
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.stoppingDistance = 0.1f;
        targetPosition = position;
        agent.SetDestination(position);
        MovementState = MovementState.Running;
        ensureRotation = false;
    }

    public void MoveCloseToPosition(Vector3 position, float distance)
    {
        agent.stoppingDistance = distance;
        targetPosition = position;
        agent.SetDestination(position);
        MovementState = MovementState.Running;
        ensureRotation = true;
    }

    public bool FaceTargetUpdate()
    {
        Vector3 targetDir = targetPosition - transform.position;
        //Removing vertical component
        targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up);

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

    public void Stop()
    {
        agent.isStopped = true;
        agent.isStopped = false;
        MovementState = MovementState.Idle;
        OnMovementCallback.Invoke();
    }
}

public enum MovementState
{
    Idle,
    Walking,
    Running,
}
