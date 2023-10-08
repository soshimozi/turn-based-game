using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject buttonHighlight;


    private BaseAction baseAction;

    public void SetBaseAction(BaseAction action)
    {
        baseAction = action;
        textMeshPro.text = action.ActionName.ToUpper();

        button.onClick.AddListener(() =>
        {
            UnitManager.Instance.SetSelectedAction(action);
        });
    }

    public void UpdateSelectedVisual()
    {
        var selectedBaseAction = UnitManager.Instance.GetSelectedAction();

        buttonHighlight.SetActive(selectedBaseAction == baseAction);
    }
}
