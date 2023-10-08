using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MinFollowYOffset = 2f;
    private const float MaxFollowYOffset = 12f;


    float rotationSpeed = 100f;
    float zoomAmount = 1f;

    float moveSpeed = 10f;
    float mouseRotationSpeed = 2f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        var inputMoveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = 1f;
        }

        if (Input.GetMouseButton(2))
        {
            inputMoveDirection.x -= Input.GetAxis("Mouse X") * mouseRotationSpeed;
            inputMoveDirection.z -= Input.GetAxis("Mouse Y") * mouseRotationSpeed;
        }


        var moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;


        var rotationVector = new Vector3(0, 0, 0);
        
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = -1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = 1;
        }

        // for zoom
        var followOffset = cinemachineTransposer.m_FollowOffset;

        // Mouse rotation
        if (Input.GetMouseButton(1))
        {
            rotationVector.y += Input.GetAxis("Mouse X") * mouseRotationSpeed;
        }


        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
        
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset.y += zoomAmount;
        }

        followOffset.y = Mathf.Clamp(followOffset.y, MinFollowYOffset, MaxFollowYOffset);
        cinemachineTransposer.m_FollowOffset = followOffset;
    }
}
