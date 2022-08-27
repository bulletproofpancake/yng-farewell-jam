using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;
    
    [SerializeField] private float offGroundTime = 3f;
    [SerializeField] private bool _isGrounded;
    private float _timeOffGround;

    public static event Action OnPlayerFall;

    Rigidbody _rb;
    Vector3 _movement;

    Animator _animator;
    
    PlayerManager _playerManager;
    PlayerInput _playerInput;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (_movement.sqrMagnitude != 0f)
        {
            float targetAngle = Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (!_isGrounded && _timeOffGround < offGroundTime)
        {
            _timeOffGround += Time.deltaTime;
        }

        if (_timeOffGround > offGroundTime)
        {
            _timeOffGround = 0;
            _playerManager.SpawnPlayer(_playerInput);
            PlayerFell();
        }

        _animator.SetBool("isMoving", _movement != Vector3.zero);
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
            _isGrounded = false;
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

    private static void PlayerFell()
    {
        OnPlayerFall?.Invoke();
    }
}
