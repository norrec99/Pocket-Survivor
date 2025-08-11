using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick _variableJoystick;

    [Header("Player Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private FindTarget _findTargetSystem;

    [SerializeField] private GameObject _joystickCanvas;

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 720f;

    private Vector3 _currentVelocity;

    private bool _isPlayerMoving = false;

    private Transform _closestEnemy;

    public static PlayerController Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnableJoystickInput();
    }

    private void EnableJoystickInput()
    {
        if (_variableJoystick != null)
        {
            _joystickCanvas.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        _closestEnemy = _findTargetSystem.GetClosestEnemy();

        PlayerMovement();
        if (_closestEnemy != null)
        {
            FocusOnTarget(_closestEnemy);
        }
        else
        {
            PlayerRotation();
        }
    }

    private void PlayerMovement()
    {
        float x_value = _variableJoystick.Horizontal;
        float z_value = _variableJoystick.Vertical;

        Vector3 moveDirection = new Vector3(x_value, 0, z_value);

        Vector3 targetVelocity = moveDirection * _moveSpeed;
        _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);
        _currentVelocity = _rigidbody.velocity;

        _isPlayerMoving = moveDirection.magnitude > 0.1f;

    }

    private void FocusOnTarget(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0; //Keep rotation horizontal.
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
    }

    private void PlayerRotation()
    {
        if (_currentVelocity.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(_currentVelocity.x, 0, _currentVelocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public bool IsPlayerMoving()
    {
        return _isPlayerMoving;
    }
}
