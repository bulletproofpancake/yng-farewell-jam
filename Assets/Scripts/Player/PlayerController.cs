using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    [Header("Ground Check")]
    [SerializeField] private float offGroundTime = 3f;
    [SerializeField] private bool _isGrounded;
    private float _timeOffGround;

    [Header("Attack")] 
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackForce = 125f;

    public static event Action OnPlayerFall;

    Rigidbody _rb;
    Vector3 _movement;

    Animator _animator;

    PlayerManager _playerManager;
    PlayerInput _playerInput;
    private PlayerControls _playerControls;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerManager = FindObjectOfType<PlayerManager>();
        _playerInput = GetComponent<PlayerInput>();
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
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
        if (!context.performed) return;
        print("Pushing");
        _animator.SetTrigger("attack");
        var collisions = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (var collision in collisions)
        {
            if (collision.CompareTag("Player") && collision.gameObject != this.gameObject)
            {
                var direction = collision.transform.position - transform.position;
                collision.GetComponent<PlayerController>().KnockbackPlayer(direction, attackForce);
            }
        }
    }

    public void KnockbackPlayer(Vector3 direction, float force)
    {
        direction.y = 0;
        _rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
    
    private static void PlayerFell()
    {
        OnPlayerFall?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}