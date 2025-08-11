using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    [SerializeField] private float _targetingRange = 10f;
    [SerializeField] private LayerMask _enemyLayer;

    private Transform _closestEnemy;

    private void FixedUpdate()
    {
        UpdateClosestEnemy();
    }

    private void UpdateClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _targetingRange, _enemyLayer);
        float closestDistance = Mathf.Infinity;
        _closestEnemy = null;

        foreach (Collider enemyCollider in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemyCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestEnemy = enemyCollider.transform;
            }
        }
    }

    public Transform GetClosestEnemy()
    {
        return _closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _targetingRange);
    }
}
