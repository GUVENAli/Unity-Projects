using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerAnimation : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Animator _animator;
    
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _sprintSpeed = 4f;
    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundDistance = 0.25f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _groundCheck;

    private Vector2 _moveInput;
    private Vector3 _verticalVeocity;

    private bool _isGrounded;
    private bool _isSprinting;
    private bool _isJumping = false;

    private void Reset()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        if (_controller == null)
        {
            _controller = GetComponent<CharacterController>();
        }

        if(_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleGroundCheck();

        ReadJoystickInput();

        HandleHorizontalMovement();

        HandleGravity();

        /*  
        // Ayný anda zýplama ve yürüme denemesi için
        if (Keyboard.current != null &&
        Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        } 
        */
    }

    private void HandleGroundCheck()
    {
        Vector3 checkPosition = _groundCheck != null ? _groundCheck.position : transform.position;
        _isGrounded = Physics.CheckSphere(checkPosition, _groundDistance, _groundMask);
        Debug.Log("Grounded: " + _isGrounded + " " + "Jumping: " + _isJumping);

        if (_isGrounded && _verticalVeocity.y < 0 && !_isJumping)
        {
            _verticalVeocity.y = -2f;
            _animator.SetBool("jump", false);
        }
        
        if (_isJumping)
        {
            _animator.SetBool("jump", true);
            if (!_isGrounded)
            {
                _verticalVeocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                _isJumping = false;
            }

        }

    }

    private void ReadJoystickInput()
    {
        if (_joystick != null)
        {
            _moveInput.x = _joystick.Horizontal;
            _moveInput.y = _joystick.Vertical;
        }
    }

    private void HandleHorizontalMovement()
    {
        Vector3 moveDirection = (transform.right * _moveInput.x + transform.forward * _moveInput.y);
        _isSprinting = _moveInput.magnitude > 0.85f;
        float currentSpeed = _isSprinting ? _sprintSpeed : _walkSpeed;
        if (currentSpeed == _walkSpeed)
        {
            _animator.SetBool("walk", true);
            _animator.SetBool("idle", false);
            _animator.SetBool("sprint", false);
        }
        else if (currentSpeed == _sprintSpeed)
        {
            _animator.SetBool("sprint", true);
            _animator.SetBool("idle", false);
            _animator.SetBool("walk", false);

        }

        if (_moveInput.magnitude == 0)
        {
            _animator.SetBool("walk", false);
            _animator.SetBool("sprint", false);

            _animator.SetBool("idle", true);
        }
        if (!_isJumping)
        {
            _controller.Move(moveDirection * (currentSpeed * Time.deltaTime));
        }

    }

    private void HandleGravity()
    {
        _verticalVeocity.y += _gravity * Time.deltaTime;
        _controller.Move(_verticalVeocity * Time.deltaTime);
    }

    public void Jump()
    {

        if (_isGrounded)
        {
            //_verticalVeocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _isJumping = true;
            _animator.SetBool("jump", true);
        }
    }
}
