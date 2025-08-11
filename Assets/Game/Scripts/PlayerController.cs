using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick _variableJoystick;

    [Header("Player Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;

    [SerializeField] private GameObject _joystickCanvas;

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 720f;

    private Vector3 _currentVelocity;

    private bool _isPlayerMoving = false;

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
        PlayerMovement();
        PlayerRotation();
    }

    private void PlayerMovement()
    {
        float x_value = _variableJoystick.Horizontal;
        float z_value = _variableJoystick.Vertical;

        Vector3 moveDirection = new Vector3(x_value, 0, z_value);

        Vector3 targetVelocity = moveDirection * _moveSpeed;
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        _currentVelocity = rb.velocity;

        _isPlayerMoving = moveDirection.magnitude > 0.1f;

    }

    private void PlayerRotation()
    {
        if (_currentVelocity.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(_currentVelocity.x, 0, _currentVelocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public bool IsPlayerMoving()
    {
        return _isPlayerMoving;
    }
}
