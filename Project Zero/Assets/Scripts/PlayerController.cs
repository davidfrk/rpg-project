using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController localPlayer;
    public Unit selectedUnit;
    private UnitController selectedUnitController;
    Vector3 targetPosition;
    Unit targetUnit;

    //Avoids clicks on UI firing commands
    UIController uiController;
    float uiProtectionTime = 0.2f;

    RaycastHit hit;
    Ray ray;
    public LayerMask layer;

    public delegate void KillEvent(Unit prey);
    public event KillEvent OnKillCallback;

    void Awake()
    {
        localPlayer = this;
        if (selectedUnit != null)
        {
            SelectUnit(selectedUnit);
        }
    }

    void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedUnitController = selectedUnit.GetComponent<UnitController>();
    }

    void Start()
    {
        uiController = UIController.instance;
    }

    void Update()
    {
        InputUpdate();
    }

    void InputUpdate()
    {
        //Avoids clicks on UI firing commands
        if (uiController.lastUIClick + uiProtectionTime > Time.time) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            targetUnit = hit.transform.GetComponentInParent<Unit>();

            //LeftClick
            if (Input.GetMouseButtonDown(0) && targetUnit != null)
            {
                SelectUnit(targetUnit);
            }

            if (selectedUnit != null && selectedUnitController.state != UnitState.Dead)
            {
                UnitCommandsUpdate();
            }
        }
    }

    void UnitCommandsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedUnitController.castController != null && selectedUnitController.castController.CanCast(0))
            {
                if (targetUnit != null)
                {
                    selectedUnitController.MoveToCast(targetUnit.transform.position, selectedUnitController.castController.GetCastRange(0));
                }
                else
                {
                    selectedUnitController.MoveToCast(hit.point, selectedUnitController.castController.GetCastRange(0));
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnitController.state != UnitState.Casting)
        {
            if (targetUnit != null)
            {
                //UnitClick
                if (selectedUnit != targetUnit)
                {
                    selectedUnitController.MoveAttack(targetUnit);
                }
            }
            else
            {
                Item item = hit.transform.GetComponentInParent<Item>();
                //PickItem
                if (item != null)
                {
                    selectedUnitController.MoveToPickItem(item);
                }
                else
                {
                    //GroundClick
                    selectedUnitController.Move(hit.point);
                }
            }
        }
    }

    public void OnKill(Unit prey)
    {
        OnKillCallback?.Invoke(prey);
    }
}
