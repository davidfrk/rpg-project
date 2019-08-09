using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRespawn : MonoBehaviour
{
    public float TimeToRespawn = 3f;

    public Vector3 SpawnPosition { get; private set; }
    Quaternion spawnOrientation;
    UnitController unitController;

    public void Awake()
    {
        unitController = GetComponent<UnitController>();
        unitController.OnDeathCallback += OnDeathCallback;
    }

    public void Start()
    {
        SpawnPosition = transform.position;
        spawnOrientation = transform.rotation;
    }

    private void OnDeathCallback(Unit killer)
    {
        Invoke("Respawn", TimeToRespawn);
    }

    public void Respawn()
    {
        unitController.Spawn(SpawnPosition, spawnOrientation);
    }
}
