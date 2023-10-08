using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.StartMoving += (sender, args) =>
            {
                animator.SetBool("IsWalking", true);
            };

            moveAction.StopMoving += (sender, args) =>
            {
                animator.SetBool("IsWalking", false);
            };
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.Shooting += (sender, args) =>
            {
                animator.SetTrigger("Shoot");

                var bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
                var bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

                var targetUnitShootAtPosition = args.TargetUnit.GetWorldPosition();
                targetUnitShootAtPosition.y = shootPointTransform.position.y;
                bulletProjectile.Setup(targetUnitShootAtPosition); 
            };
        }
    }

}
