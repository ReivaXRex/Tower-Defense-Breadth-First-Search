using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float towerRange = 15f;
    Transform target;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon(target);
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>(); // Find all the enemies in the scene and store them into an array.
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity; // Hold the current furthest distance of the enemy.

        foreach (Enemy enemy in enemies)
        {
            // Distance between the tower and the enemy
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)  
            {
                closestTarget = enemy.transform;
                // Reduce the maxDistance to the current distance of the enemy to check if the next is closer or further away.
                maxDistance = targetDistance; 
            }
        }

        target = closestTarget;
    }

    void AimWeapon(Transform aimTarget)
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(aimTarget);

        if (targetDistance < towerRange)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = isActive;
    }
}
