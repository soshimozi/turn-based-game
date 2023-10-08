using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : SingletonMonoBehaviorBase<GridSystemVisual>
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    // Start is called before the first frame update
    void Start()
    {
        gridSystemVisualSingleArray =
            new GridSystemVisualSingle[GridManager.Instance.Width, GridManager.Instance.Height];

        for (var x = 0; x < GridManager.Instance.Width; x++)
        {
            for (var z = 0; z < GridManager.Instance.Height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                var gridSystemVisualSingleTransform =
                    Instantiate(gridSystemVisualSinglePrefab, GridManager.Instance.GetWorldPosition(gridPosition),
                    Quaternion.identity);

                gridSystemVisualSingleArray[x, z] =
                    gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitManager.Instance.SelectedActionChanged += (sender, args) =>
        {
            UpdateGridVisual();
        };

        GridManager.Instance.AnyUnitChangedGridPosition += (sender, args) =>
        {
            UpdateGridVisual();
        };

        UpdateGridVisual();
    }

    // Update is called once per frame

    public void HideAllGridPositions()
    {
        for (var x = 0; x < GridManager.Instance.Width; x++)
        {
            for (var z = 0; z < GridManager.Instance.Height; z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositions(List<GridPosition> positions)
    {
        foreach (var position in positions)
        {
            gridSystemVisualSingleArray[position.x, position.z].Show();
        }
    }


    private void UpdateGridVisual()
    {
        if (!UnitManager.Instance.GetSelectedUnit()) return;

        HideAllGridPositions();
        //ShowGridPositions(UnitManager.Instance.GetSelectedUnit().GetMoveAction().GetValidActionGridPositionList());

        if (!UnitManager.Instance.GetSelectedAction()) return;

        //HideAllGridPositions();
        ShowGridPositions(UnitManager.Instance.GetSelectedAction().GetValidActionGridPositionList());

    }
}
