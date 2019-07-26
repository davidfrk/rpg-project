using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private Transform target;

    public void Start()
    {
        target = Camera.main.transform;
    }

    public void Update()
    {
        Vector3 targetPosition = target.position;
        targetPosition.x = transform.position.x;
        transform.LookAt(targetPosition,Vector3.forward);
    }
}

