using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using TreeEditor;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private const int ACTION_POINTS_MAX = 2;


    public static event EventHandler AnyActionPointsChanged;

    private BaseAction[] baseActionArray;


    [SerializeField] private bool isEnemy;
    [SerializeField] private Outlinable outlinable;
    private GridPosition gridPosition;

    private int actionPoints = ACTION_POINTS_MAX;

    private bool _canShootBullet;

    private HealthManager healthManager;


    public bool CanShootBullet
    {
        get => _canShootBullet;
        set => _canShootBullet = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        outlinable.enabled = false;

        gridPosition = GridManager.Instance.GetGridPosition(transform.position);
        GridManager.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnManager.Instance.TurnChanged += (sender, args) =>
        {
            if ((IsEnemy() && !TurnManager.Instance.IsPlayerTurn()) ||
                (!IsEnemy() && TurnManager.Instance.IsPlayerTurn()))
            {

                actionPoints = ACTION_POINTS_MAX;
                OnAnyActionPointsChanged();
            }
        };

    }

    private void Awake()
    {

        baseActionArray = GetComponents<BaseAction>();
        healthManager = GetComponent<HealthManager>();

        healthManager.UnitDied += (sender, args) =>
        {
            GridManager.Instance.RemoveUnitAtGridPosition(gridPosition, this);
            Destroy(gameObject);
        };

    }

    // Update is called once per frame
    void Update()
    {

        var newGridPosition = GridManager.Instance.GetGridPosition(transform.position);
        if (newGridPosition == gridPosition) return;

        // Unit changed Grid Position
        var oldOldGridPosition = gridPosition;
        gridPosition = newGridPosition;
        GridManager.Instance.UnitMovedGridPosition(this, oldOldGridPosition, newGridPosition);

    }


    public void Unselect()
    {
        outlinable.enabled = false;
    }

    public void Select()
    {
        outlinable.enabled = true;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged();
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }


    private static void OnAnyActionPointsChanged()
    {
        AnyActionPointsChanged?.Invoke(null, EventArgs.Empty);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }


    public void Damage(int damageAmount)
    {
        healthManager.TakeDamage(damageAmount);
    }

}
