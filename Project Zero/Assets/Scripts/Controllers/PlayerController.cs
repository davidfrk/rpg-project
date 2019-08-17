using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;
using Rpg.Skills;

public class PlayerController : MonoBehaviour
{
    static public PlayerController localPlayer;

    public CameraController mainCamera;
    public Unit selectedUnit;
    private Unit mainUnit = null;
    public Unit MainUnit
    {
        get
        {
            return mainUnit;
        }
        private set
        {
            if (mainUnit != value && value != null)
            {
                if (mainUnit != null)
                {
                    mainUnit.unitController.OnSpawnCallback -= OnRespawn;
                    mainUnit.unitController.equipmentManager.OnItemPickUpCallback -= OnItemPickUp;
                }
                mainUnit = value;
                mainUnit.unitController.OnSpawnCallback += OnRespawn;
                mainUnit.unitController.equipmentManager.OnItemPickUpCallback += OnItemPickUp;
            }
        }
    }

    private UnitController selectedUnitController;
    private Vector3 targetPosition;
    private Unit targetUnit;

    public int gold = 0;

    RaycastHit hit;
    Ray ray;
    public LayerMask layer;

    public delegate void KillEvent(Unit prey);
    public event KillEvent OnKillCallback;

    public delegate void PickUpEvent(Item item);
    public event PickUpEvent OnItemPickUpCallback;

    void Awake()
    {
        localPlayer = this;
    }

    void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedUnitController = selectedUnit.GetComponent<UnitController>();
        if (selectedUnitController.isPlayerUnit)
        {
            MainUnit = selectedUnit;
        }
    }

    void Start()
    {
        if (selectedUnit != null)
        {
            SelectUnit(selectedUnit);
        }
    }

    void Update()
    {
        InputUpdate();
    }

    void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.CenterOnUnit(mainUnit);
            SelectUnit(mainUnit);
        }

        //Avoids clicks on UI firing commands
        if (UIController.UIProtectionTime()) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            targetUnit = hit.transform.GetComponentInParent<Unit>();

            //LeftClick
            if (Input.GetMouseButtonDown(0) && targetUnit != null)
            {
                SelectUnit(targetUnit);
            }

            if (selectedUnit != null && selectedUnitController.State != UnitState.Dead)
            {
                UnitCommandsUpdate();
            }
        }
    }

    void UnitCommandsUpdate()
    {
        CastInputUpdate();

        if (Input.GetMouseButtonDown(1))
        {
            if (!selectedUnit.unitController.isPlayerUnit)
            {
                SelectUnit(mainUnit);
            }

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

    void CastInputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cast(0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Cast(1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Cast(2);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Cast(3);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Cast(GoBaseSkill);
        }
    }

    void Cast(int slot)
    {
        if (!selectedUnit.unitController.isPlayerUnit)
        {
            SelectUnit(mainUnit);
        }

        if (selectedUnitController.castController != null && selectedUnitController.castController.CanCast(slot))
        {
            Skill skill = selectedUnitController.castController.skills[slot];

            if (skill.forceSelfCast)
            {
                selectedUnitController.MoveToCast(mainUnit, slot);
            }
            else if (targetUnit != null)
            {
                if (skill.canCastOnUnit)
                {
                    selectedUnitController.MoveToCast(targetUnit, slot);
                }
                else
                {
                    selectedUnitController.MoveToCast(targetUnit.transform.position, slot);
                }
            }
            else
            {
                if (skill.canCastOnGround)
                {
                    selectedUnitController.MoveToCast(hit.point, slot);
                }
            }
        }
    }

    public void OnKill(Unit prey)
    {
        OnKillCallback?.Invoke(prey);
    }

    //Esses eventos deveriam ser recebidos diretamente pelos interessados
    void OnRespawn(Unit unit)
    {
        mainCamera.CenterOnUnit(unit);
    }

    void OnItemPickUp(Item item)
    {
        OnItemPickUpCallback?.Invoke(item);
    }
}
