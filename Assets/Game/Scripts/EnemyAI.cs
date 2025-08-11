using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private Transform _target;
    private Rigidbody _aiBody;

    private float _currentSpeed;

    void Awake()
    {
        _aiBody = GetComponent<Rigidbody>();
        _currentSpeed = _moveSpeed;
    }

    void Start()
    {
        _target = PlayerController.Instance.transform;
    }

    private void FixedUpdate()
    {
        EnemyMove(_target.position);
    }

    public void EnemyMove(Vector3 target)
    {
        Vector3 newTarget = new(target.x, transform.position.y, target.z);

        Quaternion targetRotation = Quaternion.LookRotation(newTarget - transform.position, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

        _aiBody.velocity = transform.forward * _currentSpeed;
    }
}
