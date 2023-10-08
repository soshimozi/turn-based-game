using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private GameObject busyFrame;

    // Start is called before the first frame update
    void Start()
    {
        busyFrame.SetActive(false);
        UnitManager.Instance.BusyStarted += (sender, b) =>
        {
            busyFrame.SetActive(b);
        };
    }

}
