using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rayRange = 5f;
    private float _gravity = 9.81f;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CharacterController _characterController;

    private Vector2 _moveInput;
    private float _verticalVelocity;

    private void Reset()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        if (_characterController != null) _characterController = GetComponent<CharacterController> ();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleJoystick();
    }

    private void HandleJoystick()
    {
        if (_joystick == null) return;
        else
        {
            _moveInput.x = _joystick.Horizontal;
            _moveInput.y = _joystick.Vertical;
        }
        Vector3 moveDirection = (-transform.right * _moveInput.x - transform.forward * _moveInput.y);

        if (_characterController.isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
        }

        moveDirection.y = _verticalVelocity;

        _characterController.Move(moveDirection * (_moveSpeed * Time.deltaTime));
    }
}
