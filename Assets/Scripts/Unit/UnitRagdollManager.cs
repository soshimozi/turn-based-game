using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollManager : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootbone;

    private HealthManager healthManager;


    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();

        healthManager.UnitDied += HealthManager_OnUnitDied;
    }

    private void HealthManager_OnUnitDied(object sender, EventArgs args)
    {
        var ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        ragdollTransform.GetComponent<UnitRagdoll>().Setup(originalRootbone);
    }
}
