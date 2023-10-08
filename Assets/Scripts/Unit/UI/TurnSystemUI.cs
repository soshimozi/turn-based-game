using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;


    // Start is called before the first frame update
    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnManager.Instance.NextTurn();
        });

        TurnManager.Instance.TurnChanged += (sender, args) =>
        {
            UpdateTurnText();
            UpdateEndTurnButtonVisibility();
        };

        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = $"TURN {TurnManager.Instance.GetTurnNumber()}";
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnManager.Instance.IsPlayerTurn());
    }

}
