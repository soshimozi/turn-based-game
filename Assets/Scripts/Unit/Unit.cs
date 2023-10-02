using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(SteeringBehavior))]
public class Unit : MonoBehaviour
{

    private Vector3 targetPosition;
    private SteeringBehavior _steeringBehavior;

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Animator unitAnimator;
    private GridPosition gridPosition;

    private void Aawake()
    {
        targetPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        _steeringBehavior = GetComponent<SteeringBehavior>();

        gridPosition = GridManager.instance.GetGridPosition(transform.position);
        GridManager.instance.AddUnitAtGridPosition(gridPosition, this);
    }


    // Update is called once per frame
    void Update()
    {
        var currentSpeed = 0f;

        // TODO: call correct behavior based on our state later in game
        if (!_steeringBehavior.Arrive(targetPosition, out var newPosition))
        {
            transform.position += newPosition * Time.deltaTime * speed;

            transform.forward = Vector3.Lerp(transform.forward, (targetPosition - transform.position).normalized,
                Time.deltaTime * rotateSpeed);

            currentSpeed = newPosition.magnitude;
        }

        unitAnimator.SetFloat("Speed", currentSpeed);

        GridPosition newGridPosition = GridManager.instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            GridManager.instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }

    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

}
