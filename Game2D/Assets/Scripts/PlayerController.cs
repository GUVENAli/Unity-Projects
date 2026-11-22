using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float targetArriveThreshold = 1.5f;
    [SerializeField] private float targetDirectionAngle = 20f;
    [SerializeField] private float avoidRadius = 0.2f; 
    [SerializeField] private float avoidDistance = 0.45f;

    private Rigidbody2D rb;
    private Animator animator;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction clickAction;

    private Vector2 moveInput;
    private Vector2 targetPos;
    private bool hasTarget = false;
    private Vector2 wallNormal = Vector2.zero; // Deðdiðimiz duvarýn itme yönü
    private bool isTouchingWall = false;

    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int isWalkingHash = Animator.StringToHash("walk");
    private static readonly int AttackHash = Animator.StringToHash("attack");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        moveAction.AddBinding("<Gamepad>/leftStick");

        clickAction = new InputAction("Click", binding: "<Mouse>/leftButton");
        clickAction.AddBinding("<Touchscreen>/primaryTouch/press"); // Simulator ekranýnda anýnda algýlar
        clickAction.AddBinding("<Pointer>/press");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        clickAction.Enable();
        //attackAction.Enable();
        //attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        clickAction.Disable();
        //attackAction.Disable();
        //attackAction.performed -= OnAttackPerformed;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ekrana bakar halde baþlat
        animator.SetFloat(MoveXHash, 0);
        animator.SetFloat(MoveYHash, -1);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if(moveInput.sqrMagnitude > 0.01f)
        {
            hasTarget = false;
        }
        else
        {
            if (clickAction.WasPressedThisFrame() && !IsPointerOverUI())
            {
                Vector2 screenPos = Pointer.current.position.ReadValue();
                Vector3 screenPos3D = new Vector3(screenPos.x, screenPos.y, Mathf.Abs(Camera.main.transform.position.z));
                targetPos = Camera.main.ScreenToWorldPoint(screenPos);
                hasTarget = true;
            }

            if(hasTarget)
            {
                Vector2 toTarget = targetPos - (Vector2)transform.position;
                Debug.Log("Dist: " + toTarget.magnitude);

                if(toTarget.magnitude < targetArriveThreshold) // hedefe varma
                {
                    hasTarget = false;
                    moveInput = Vector2.zero;
                }
                else
                {
                    Vector2 dir = toTarget.normalized;

                    if(isTouchingWall)
                    {
                        Vector2 slideRight = new Vector2(wallNormal.y, -wallNormal.x);
                        Vector2 slideLeft = new Vector2(-wallNormal.y, wallNormal.x);

                        // Hedefe daha yakýn olan kayma yönünü seç
                        dir = Vector2.Dot(slideRight, toTarget) > Vector2.Dot(slideLeft, toTarget) ? slideRight : slideLeft;

                    }

                    // Önünde engel varsa saða veya sola kýr
                    else if (Physics2D.CircleCast(transform.position, avoidRadius, dir, avoidDistance, obstacleLayer))
                    {
                        Vector2 right = Quaternion.Euler(0, 0, -targetDirectionAngle) * dir;
                        Vector2 left = Quaternion.Euler(0, 0, targetDirectionAngle) * dir;

                        if (!Physics2D.CircleCast(transform.position, avoidRadius, right,avoidDistance, obstacleLayer)) dir = right;
                        else if (!Physics2D.CircleCast(transform.position, avoidRadius, left, avoidDistance, obstacleLayer)) dir = left;
                    }

                    moveInput = dir.normalized;
                }
            }
        }

        if(moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool(isWalkingHash, isMoving);

        if (isMoving)
        {
            Debug.Log("Target: " + hasTarget);
            animator.SetFloat(MoveXHash, moveInput.x);
            animator.SetFloat(MoveYHash, moveInput.y);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger(AttackHash);
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            isTouchingWall = true;
            wallNormal = collision.GetContact(0).normal;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0) // engel layerý olup olmadýðý
        {
            isTouchingWall = false;
            wallNormal = Vector2.zero;
        }
    }
}
