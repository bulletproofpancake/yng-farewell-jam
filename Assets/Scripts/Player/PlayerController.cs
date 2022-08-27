using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    Rigidbody _rb;
    Vector3 _movement;

    Animator _animator;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_movement.sqrMagnitude != 0f)
        {
            float targetAngle = Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        _animator.SetBool("isMoving", _movement != Vector3.zero);
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movement.x = context.ReadValue<Vector2>().x;
        _movement.z = context.ReadValue<Vector2>().y;
        _movement.Normalize();
    }
    public void Push(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("attack");
    }
}
