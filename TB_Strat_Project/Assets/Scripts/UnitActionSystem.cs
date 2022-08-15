using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] Unit selectedUnit = null;
    [SerializeField] LayerMask unitLayerMask = new LayerMask();

    BaseAction selectedAction = null;
    bool isBusy = false;

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError($"There is more than one UnitActionSystem! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    void Update()
    {
        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) 
                return;
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                return;

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit) 
                        return false;

                    if (unit.IsEnemy())
                        return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        if (selectedUnit == null)
            if (UnitManager.Instance.GetFriendlyUnitList().Count > 0)
                SetSelectedUnit(UnitManager.Instance.GetFriendlyUnitList()[0]);
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
