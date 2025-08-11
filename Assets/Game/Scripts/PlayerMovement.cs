using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private VariableJoystick _variableJoystick;

    [SerializeField] private CharacterController _characterController;

    [SerializeField] private GameObject _joystickCanvas;

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 20f;

    private bool _isJoystickActive = false;

    private Vector3 _moveDirection;
    private Vector3 _rotationDirection;

    private void Start()
    {
        EnableJoystickInput();
    }

    private void EnableJoystickInput()
    {
        if (_variableJoystick != null)
        {
            _isJoystickActive = true;
            _joystickCanvas.SetActive(true);
        }
    }

    private void Update()
    {
        _moveDirection = new Vector3(_variableJoystick.Horizontal, 0, _variableJoystick.Vertical);

        _isJoystickActive = _moveDirection.sqrMagnitude > 0;

        if (_isJoystickActive)
        {
            _characterController.Move(_moveDirection * _moveSpeed * Time.deltaTime);

            _rotationDirection = Vector3.RotateTowards(
                _characterController.transform.forward,
                _moveDirection,
                _rotationSpeed * Time.deltaTime,
                0.0f
            );
            _characterController.transform.rotation = Quaternion.LookRotation(_rotationDirection);
        }
    }
}
