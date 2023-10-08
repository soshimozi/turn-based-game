using System;
using System.Collections;
using System.Collections.Generic;
using DTT.AreaOfEffectRegions;
using UnityEngine;

[RequireComponent(typeof(CircleRegion))]
public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private CircleRegion region;

    //private MeshRenderer meshRenderer;

    private void Awake()
    {
        region = GetComponent<CircleRegion>();
        //meshRenderer = GetComponent<MeshRenderer>();
    }



    // Start is called before the first frame update
    void Start()
    {

        UnitManager.Instance.SelectedUnitChanged += UnitManager_OnSelectedUnitChanged;

        UpdateVisual();
    }

    private void UnitManager_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        if (UnitManager.Instance.GetSelectedUnit() == unit)
        {
            region.gameObject.SetActive(true);
        }
        else
        {
            region.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        UnitManager.Instance.SelectedUnitChanged -= UnitManager_OnSelectedUnitChanged;
    }

}
