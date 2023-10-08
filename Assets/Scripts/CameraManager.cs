using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.AnyActionStarted += (sender, args) =>
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    var shootUnit = shootAction.Unit;
                    var targetUnit = shootAction.TargetUnit;
                    var cameraCharacterHeight = Vector3.up * 1.6f;

                    var shootDir = (targetUnit.GetWorldPosition() - shootUnit.GetWorldPosition()).normalized;

                    float shoulderOffsetAmmount = -.5f;
                    var shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmmount;

                    Vector3 actionCameraPosition = shootUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset +
                                               (shootDir * -1);

                    actionCameraGameObject.transform.position = actionCameraPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);


                    ShowActionCamera();
                    break;

                default:
                    break;
            }
        };

        BaseAction.AnyActionComplete += (sender, args) =>
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    HideActionCamera();
                    break;

                default:
                    break;
            }
        };
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

}
