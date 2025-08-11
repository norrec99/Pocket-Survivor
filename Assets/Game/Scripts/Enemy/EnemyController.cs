using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float hp = 100f;

    private Transform target;
    private Rigidbody aiBody;

    private float currentSpeed;
    private float currentHp;

    void Awake()
    {
        aiBody = GetComponent<Rigidbody>();
        currentSpeed = moveSpeed;
    }

    void OnEnable()
    {
        ResetEnemy();
    }

    void Start()
    {
        target = PlayerController.Instance.transform;
    }

    private void FixedUpdate()
    {
        EnemyMove(target.position);
    }

    public void EnemyMove(Vector3 target)
    {
        Vector3 newTarget = new(target.x, transform.position.y, target.z);

        Quaternion targetRotation = Quaternion.LookRotation(newTarget - transform.position, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        aiBody.velocity = transform.forward * currentSpeed;
    }

    public void EnemyTakeDamage(float playerDamage)
    {
        if (currentHp <= 0) return;

        currentHp -= playerDamage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        aiBody.velocity = Vector3.zero;
        aiBody.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void ResetEnemy()
    {
        currentHp = hp;
        currentSpeed = moveSpeed;
    }
}
