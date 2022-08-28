using System;
using System.Collections;
using System.Diagnostics.Tracing;
using Interfaces;
using UnityEditor.Searcher;
using UnityEngine;

[SelectionBase]
public class PlayerModel : MonoBehaviour
{
    public event Action<float> OnMoveUpdate;
    public event Action<bool> OnJumpUpdate;
    public event Action<bool> OnCrouchUpdate;

    [SerializeField] private PlayerData data;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private Transform groundCheckPoint;

    private bool _isJumping, _isLookingRight, _isGrounded;
    private float _currentSpeed, _crouchSpeedMultiplier, _currentGravity, _moveDirCached;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private Collider2D[] _interactablesArray = new Collider2D[2];
    private Collider2D[] _groundedArray = new Collider2D[2];
    private float _currentCoyoteTime = 0;
    private int _currentJumps;
    private float _lastDash;
    private float _initialColliderHeight, _initialColliderOffset;
    private bool _gravityEnabled;
    public PlayerController Controller { get; private set; }


    #region Attributes

    // All attributes that can be modified by external sources

    private int _maxJumps = 1;
    private float _dashTime = 1f;

    // Attribute setters

    public int AddJumps(int newJumps) => _maxJumps += newJumps;
    public void AddDashForce(float time) => _dashTime += time;

    #endregion

    public void SubscribeToController(PlayerController controller)
    {
        controller.OnCrouch += CrouchHandler;
        controller.OnInteract += InteractHandler;
        controller.OnJump += JumpHandler;
        controller.OnDash += DashHandler;
        controller.OnMove += MoveHandler;
        controller.OnRun += RunHandler;
        controller.OnOpenInventory += OpenInventoryHandler;
        Controller = controller;
    }

    private void Awake()
    {
        GetReferences();
        Initialize();
    }

    private void Initialize()
    {
        _dashTime = data.dashTime;
        _currentSpeed = data.walkSpeed;
        _isLookingRight = true;
        _crouchSpeedMultiplier = 1f;
        _currentGravity = data.FallGravity;
        _currentCoyoteTime = data.coyoteTime;
        _initialColliderHeight = _collider.size.y;
        _initialColliderOffset = _collider.offset.y;
        SetGravityEnabled(true);
        SetColliderData(_initialColliderHeight, _initialColliderOffset);
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        GetComponent<PlayerView>()?.SubscribeToEvents(this);
    }

    private void SetColliderData(float colliderHeightY, float colliderOffsetY)
    {
        var size = _collider.size;
        size.y = colliderHeightY;
        _collider.size = size;

        var offset = _collider.offset;
        offset.y = colliderOffsetY;
        _collider.offset = offset;
    }

    private void ApplyCustomGravity()
    {
        if (!_gravityEnabled)
            return;
        if (_rb.velocity.y < data.maxFallSpeed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, data.maxFallSpeed);
            return;
        }

        _rb.AddForce(Vector2.up * (_currentGravity * Time.fixedDeltaTime), ForceMode2D.Force);
    }


    private void SetGravityEnabled(bool gravityIsEnabled)
    {
        _gravityEnabled = gravityIsEnabled;
    }

    private void OpenInventoryHandler(bool obj)
    {
    }

    private void RunHandler(bool isRunning)
    {
        _currentSpeed = isRunning ? data.runSpeed : data.walkSpeed;
    }

    private void MoveHandler(float moveDir)
    {
        _moveDirCached = moveDir;
    }

    private void FixedUpdate()
    {
        PhysicsMovement();
        ApplyCustomGravity();
        CheckGroundUpdate();
    }

    private void CheckGroundUpdate()
    {
        _isGrounded = CheckGrounded();
        if (!_isGrounded)
        {
            _currentCoyoteTime -= Time.fixedDeltaTime;
        }
    }


    private void PhysicsMovement()
    {
        var currentSpeed = _currentSpeed * _crouchSpeedMultiplier;
        OnMoveUpdate?.Invoke(_moveDirCached * _currentSpeed);

        if (_moveDirCached == 0)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            return;
        }

        _rb.velocity = _rb.velocity.ModifyXAxis(currentSpeed * _moveDirCached);
        var lookRight = _moveDirCached > 0;
        if (lookRight != _isLookingRight)
        {
            FlipCharacter();
        }

        _moveDirCached = 0;
    }

    private void FlipCharacter()
    {
        _isLookingRight = !_isLookingRight;
        var transform1 = transform;
        var scale = transform1.localScale;
        scale.x *= -1;
        transform1.localScale = scale;
    }

    private bool CheckGrounded()
    {
        return Physics2D.OverlapBoxNonAlloc(groundCheckPoint.position, data.groundCheckBoxSize, 0, _groundedArray,
            data.groundCheckLayerMask) != 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!GameStaticFunctions.IsGoInLayerMask(col.gameObject, data.groundCheckLayerMask)) return;

        OnTouchFloor();
    }

    private void JumpHandler(bool isButtonPressed)
    {
        if (isButtonPressed && (_isGrounded || (_currentCoyoteTime > 0 && _currentJumps > 0)))
        {
            _currentJumps--;
            StartJump();
            return;
        }

        EndJump();
    }

    private void StartJump()
    {
        _currentGravity = data.jumpGravityValue;
        if (_rb.velocity.y < 0)
            _rb.velocity = _rb.velocity.ModifyYAxis(0);
        _rb.AddForce(Vector2.up * data.jumpInitialForce, ForceMode2D.Impulse);
    }

    private void EndJump()
    {
        //  print("End jump");
        if (_rb.velocity.y > 0 && data.instantFalling)
        {
            _rb.velocity = _rb.velocity.ModifyYAxis(0);
        }

        _currentGravity = data.FallGravity;
    }

    private void DashHandler()
    {
        if (_lastDash > Time.time)
            return;

        _lastDash = Time.time + data.dashCooldown;
        Controller.EnableInput(false);
        StartDash();
    }

    private void StartDash()
    {
        var direction = _isLookingRight ? 1 : -1;
        _rb.velocity = Vector2.zero;
        SetGravityEnabled(false);
        StartCoroutine(DashCoroutine(direction));
    }


    private IEnumerator DashCoroutine(float dirToMoveX)
    {
        var dashCurrentTime = 0f;
        var playerRight = transform.right;
        while (dashCurrentTime < _dashTime)
        {
            var deltaT = Time.deltaTime;
            transform.position += playerRight * (dirToMoveX * deltaT * data.dashInitialForce);
            dashCurrentTime += deltaT;
            yield return null;
        }

        SetGravityEnabled(true);
        Controller.EnableInput(true);
    }

    private void InteractHandler()
    {
        var interactablesFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, data.interactionRadius,
            _interactablesArray, data.interactionLayers);

        if (interactablesFound < 1)
            return;

        var interactable = _interactablesArray[0].GetComponent<IInteractable>();
        interactable?.OnInteract(this);
    }


    private void CrouchHandler(bool isCrouching)
    {
        _crouchSpeedMultiplier = isCrouching ? data.crouchSpeedMultiplier : 1;
        var size = isCrouching ? data.crouchColliderSizeY : _initialColliderHeight;
        var offset = isCrouching ? data.crouchColliderOffsetY : _initialColliderOffset;
        SetColliderData(size, offset);
        OnCrouchUpdate?.Invoke(isCrouching);
    }

    private void OnTouchFloor()
    {
        _isGrounded = true;
        _currentJumps = _maxJumps;
        _currentCoyoteTime = data.coyoteTime;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null || groundCheckPoint == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPoint.position, data.interactionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, data.groundCheckBoxSize);
    }
#endif
}