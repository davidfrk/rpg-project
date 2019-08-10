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
    private Unit mainUnit;
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
                }
                mainUnit = value;
                mainUnit.unitController.OnSpawnCallback += OnRespawn;
            }
        }
    }

    private UnitController selectedUnitController;
    private Vector3 targetPosition;
    private Unit targetUnit;

    public int gold = 0;

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
        if (selectedUnitController.playerUnit)
        {
            MainUnit = selectedUnit;
        }
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

            if (selectedUnit != null && selectedUnitController.State != UnitState.Dead)
            {
                UnitCommandsUpdate();
            }
        }
    }

    void UnitCommandsUpdate()
    {
        if (!Debug.isDebugBuild && !selectedUnit.unitController.playerUnit)
        {
            return;
        }

        CastInputUpdate();

        if (Input.GetMouseButtonDown(1))
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.CenterOnUnit(mainUnit);
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

    void OnRespawn(Unit unit)
    {
        mainCamera.CenterOnUnit(unit);
    }

    public void OnKill(Unit prey)
    {
        OnKillCallback?.Invoke(prey);
    }
}
