using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    [SerializeField] private LayerMask mousePlaneLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
    }

    public static bool GetPosition(out Vector3 point)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var result = Physics.Raycast(ray, out var objectHit, float.MaxValue, instance.mousePlaneLayerMask);
        point = objectHit.point;

        return result;

    }
}
