using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static public CameraController mainCamera;
    public float maxDistance = 20f;
    public float gripSensitivity = 1f;
    public bool edgePanActive = true;
    public float edgeSensitivity = 1f;
    public float height = 5f;
    public float displacement = 1f;

    bool grip = false;
    Vector3 gripStartingPosition;
    Vector3 cameraGripStartingPosition;

    void Awake()
    {
        mainCamera = this;
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
        else EdgeUpdate();
    }

    void GripUpdate()
    {
        Vector3 displacement = Input.mousePosition - gripStartingPosition;
        displacement = new Vector3(displacement.x, 0f, displacement.y);

        Vector3 newPosition = cameraGripStartingPosition + gripSensitivity * displacement;

        float distance = XZDistance(transform.position, PlayerController.localPlayer.MainUnit.transform.position);
        float newDistance = XZDistance(newPosition, PlayerController.localPlayer.MainUnit.transform.position);
        if (newDistance < maxDistance || newDistance <= distance)
        {
            transform.position = newPosition;
        }
        else
        {
            gripStartingPosition = Input.mousePosition;
            cameraGripStartingPosition = transform.position;
        }
    }

    void EdgeUpdate()
    {
        if (!edgePanActive) return;

        Vector2 dir = Vector2.zero;

        if (Input.mousePosition.x <= 1)
        {
            dir.x = -1f;
        }else if (Input.mousePosition.x >= Screen.width - 2)
        {
            dir.x = 1f;
        }

        if (Input.mousePosition.y <= 1)
        {
            dir.y = -1f;
        }
        else if (Input.mousePosition.y >= Screen.height - 2)
        {
            dir.y = 1f;
        }

        if (dir != Vector2.zero)
        {
            dir.Normalize();
            Vector3 newPosition = transform.position + Time.deltaTime * edgeSensitivity * new Vector3(dir.x, 0f, dir.y);

            float distance = XZDistance(transform.position, PlayerController.localPlayer.MainUnit.transform.position);
            float newDistance = XZDistance(newPosition, PlayerController.localPlayer.MainUnit.transform.position);
            if (newDistance < maxDistance || newDistance <= distance)
            {
                transform.position = newPosition;
            }
        }
    }

    public void CenterOnUnit(Unit unit)
    {
        transform.position = new Vector3(unit.transform.position.x, transform.position.y, unit.transform.position.z - 6f);
        gripStartingPosition = Input.mousePosition;
        cameraGripStartingPosition = transform.position;
    }

    private float XZDistance(Vector3 pointA, Vector3 pointB)
    {
        return (new Vector3(pointA.x, 0f, pointA.z) - new Vector3(pointB.x, 0f, pointB.z)).magnitude;
    }
}
