using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Transform actionButtonVisual;

    private List<ActionButtonUI> actionButtonUIList;

    void Awake()
    {
        actionButtonUIList = new();
    }

    // Start is called before the first frame update
    void Start()
    {

        gameObject.SetActive(false);

        UnitManager.Instance.SelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitManager.Instance.SelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitManager.Instance.ActionStarted += UnitActionSystem_OnActionStarted;
        TurnManager.Instance.TurnChanged += TurnManager_OnTurnChanged;
        Unit.AnyActionPointsChanged += Unit_OnAnyActionPointsChanged;


        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }


    private void TurnManager_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
        UpdateActionButtonVisual();
    }

    private void UpdateActionButtonVisual()
    {
        actionButtonVisual.gameObject.SetActive(TurnManager.Instance.IsPlayerTurn());
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnActionStarted(object send, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        var selectedUnit = UnitManager.Instance.GetSelectedUnit();
        gameObject.SetActive(selectedUnit != null);

        ClearActionButtons();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (var actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        var selectedUnit = UnitManager.Instance.GetSelectedUnit();

        if (selectedUnit == null) actionPointsText.enabled = false;
        else
        {
            actionPointsText.enabled = true;
            actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }
    }

    private void ClearActionButtons()
    {
        foreach (Transform transform in actionButtonContainer)
        {
            Destroy(transform.gameObject);
        }
        actionButtonUIList.Clear();
    }

    private void CreateUnitActionButtons()
    {
        var selectedUnit = UnitManager.Instance.GetSelectedUnit();

        if (selectedUnit == null) return;

        foreach (var action in selectedUnit.GetBaseActionArray())
        {
            var actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            var actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();

            actionButtonUI.SetBaseAction(action);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

}
