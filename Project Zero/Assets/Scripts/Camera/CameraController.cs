using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1f;
    public float height = 5f;
    public float displacement = 1f;

    bool grip = false;
    Vector3 gripStartingPosition;
    Vector3 cameraGripStartingPosition;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            grip = true;
            gripStartingPosition = Input.mousePosition;
            cameraGripStartingPosition = transform.position;
        }

        if (Input.GetMouseButtonUp(2))
        {
            grip = false;
        }

        if (grip) GripUpdate();
    }

    void GripUpdate()
    {
        Vector3 displacement = Input.mousePosition - gripStartingPosition;
        displacement = new Vector3(displacement.x, 0f, displacement.y);

        transform.position = cameraGripStartingPosition + sensitivity * displacement;
    }

    public void CenterOnUnit(Unit unit)
    {
        transform.position = new Vector3(unit.transform.position.x, transform.position.y, unit.transform.position.z - 6f);
    }
}
