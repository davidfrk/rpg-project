using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool playerUnit = false;

    UnitController unitController;
    Vector3 targetPosition;
    Unit targetUnit;

    //Avoids clicks on UI firing commands
    UIController uiController;
    float uiProtectionTime = 0.2f;

    RaycastHit hit;
    Ray ray;
    public LayerMask layer;

    void Awake()
    {
        unitController = GetComponent<UnitController>();
    }

    void Start()
    {
        uiController = UIController.instance;
    }

    void Update()
    {
        if (unitController.state == UnitState.Dead) return;

        if (playerUnit)
        {
            InputUpdate();
        }
    }

    void InputUpdate()
    {
        //Avoids clicks on UI firing commands
        if (uiController.lastUIClick + uiProtectionTime > Time.time) return;

        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                targetUnit = hit.transform.GetComponentInParent<Unit>();
                if (targetUnit != null)
                {
                    //UnitClick
                    if (unitController.unit != targetUnit)
                    {
                        unitController.MoveAttack(targetUnit);
                    }
                }
                else
                {
                    Item item = hit.transform.GetComponentInParent<Item>();
                    //PickItem
                    if (item != null)
                    {
                        unitController.MoveToPickItem(item);
                    }
                    else
                    {
                        //GroundClick
                        unitController.Move(hit.point);
                    }
                }
            }
        }
    }
}
