using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

[Serializable]
public class Socket
{
    public string socketName;
    public Transform transformObject;
}

[SelectionBase]
public class PlayerModel : MonoBehaviour
{
    public event Action<float> OnMoveUpdate;
    public event Action<bool> OnJumpUpdate;
    public event Action<bool> OnCrouchUpdate;
    public event Action OnDoubleJump;

    public event Action<bool> OnGroundedUpdate;
    public event Action<bool> OnGlidingUpdate;
    public event Action<bool> OnDashUpdate;

    [SerializeField] private PlayerData data;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private List<Socket> playerSockets;
    public PlayerController Controller { private set; get; }

    private bool _isJumping, _isLookingRight, _isGrounded, _isGliding;
    private float _currentSpeed, _crouchSpeedMultiplier, _currentGravity, _moveDirCached;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private Collider2D[] _interactablesArray = new Collider2D[2];
    private Collider2D[] _groundedArray = new Collider2D[2];
    private RaycastHit2D[] _checkCollisionsAbove = new RaycastHit2D[1];
    private float _currentCoyoteTime = 0;
    private int _currentJumps;
    private float _lastDash;
    private float _initialColliderHeight, _initialColliderOffset;
    private bool _gravityEnabled;
    private bool _isPreventedFromUncrouching;
    private Coroutine _glidingCoroutine;
    private Dictionary<string, Transform> _equipableItemsPositionBySocket;

    #region Attributes

    // All attributes that can be modified by external sources

    private int _maxJumps = 1;
    private float _dashTime = 1f;
    private bool _isDashing;
    private bool _canGlide;
    private bool _canDash;
    private bool _canBigPush;

    public void AddMaxJumps(int jumpsToAdd) => _maxJumps += jumpsToAdd;
    public void AddDashTime(float extraDashTime) => _dashTime = extraDashTime;
    public void SetGlideAbility(bool canGlide) => _canGlide = canGlide;
    public void SetDashAbility(bool canDash) => _canDash = canDash;

#if UNITY_EDITOR

    /// <summary>
    /// For Testing only EDITOR ONLY
    /// </summary>
    [ContextMenu("Set can glide bool")]
    public void SetCanGlide()
    {
        _canGlide = true;
    }

    /// <summary>
    /// For Testing only EDITOR ONLY
    /// </summary>
    [ContextMenu("Set double jump")]
    public void SetDoubleJump()
    {
        _maxJumps += 1;
    }
#endif

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

    public Transform GetSocket(string socketName)
    {
        return _equipableItemsPositionBySocket.TryGetValue(socketName, out var socket) ? socket : default;
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
        _equipableItemsPositionBySocket = new Dictionary<string, Transform>();

        foreach (var socket in playerSockets)
        {
            _equipableItemsPositionBySocket.Add(socket.socketName, socket.transformObject);
        }
    }

    public List<string> GetAllSockets()
    {
        return playerSockets.Select(t => t.socketName).ToList();
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
        var previousGroundedState = _isGrounded;
        var newGrounded = CheckGrounded();

        if (previousGroundedState != newGrounded)
        {
            _isGrounded = newGrounded;
        }

        OnGroundedUpdate?.Invoke(_isGrounded);
        if (!_isGrounded)
        {
            _currentCoyoteTime -= Time.fixedDeltaTime;
        }
    }


    private void PhysicsMovement()
    {
        if (_isDashing)
        {
            return;
        }

        if (_isPreventedFromUncrouching && !CheckCollisionAbove())
        {
            CrouchHandler(false);
            _isPreventedFromUncrouching = false;
        }

        var currentSpeed = _currentSpeed * _crouchSpeedMultiplier;
        OnMoveUpdate?.Invoke(_moveDirCached != 0 ? 1 * _currentSpeed : 0);


        if (_moveDirCached == 0)
        {
            _rb.velocity = _rb.velocity.ModifyXAxis(0);
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
        if (isButtonPressed && (_isGrounded || ((_currentCoyoteTime > 0 || _maxJumps > 1) && _currentJumps > 0)))
        {
            _currentJumps--;
            StartJump();
            return;
        }

        if (_canGlide && isButtonPressed && !_isGrounded && !_isGliding)
        {
            _isGliding = true;
            _glidingCoroutine = StartCoroutine(GlidingTimer());
            return;
        }

        EndJump();
    }

    private IEnumerator GlidingTimer()
    {
        _currentGravity = data.glidingGravity;
        OnGlidingUpdate?.Invoke(true);
        _rb.velocity = _rb.velocity.ModifyYAxis(0);
        var counter = 0f;

        while (counter < data.glidingTime)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        _isGliding = false;
        OnGlidingUpdate?.Invoke(false);
        _glidingCoroutine = null;
        EndJump();
    }

    private void StartJump()
    {
        _currentGravity = data.jumpGravityValue;
        OnJumpUpdate?.Invoke(true);
        //Which means we jumped more than once in the air
        if (_maxJumps > 1 && _currentJumps == 0)
        {
            OnDoubleJump?.Invoke();
        }

        CrouchHandler(false);
        if (_rb.velocity.y < 0)
            _rb.velocity = _rb.velocity.ModifyYAxis(0);
        _rb.AddForce(Vector2.up * data.jumpInitialForce, ForceMode2D.Impulse);
    }

    private void EndJump()
    {
        if (_rb.velocity.y > 0 && data.instantFalling)
        {
            _rb.velocity = _rb.velocity.ModifyYAxis(0);
        }

        if (_glidingCoroutine != null)
        {
            StopCoroutine(_glidingCoroutine);
            _isGliding = false;
            OnGlidingUpdate?.Invoke(false);
        }

        OnJumpUpdate?.Invoke(false);

        _currentGravity = data.FallGravity;
    }

    private void DashHandler()
    {
        if (!_canDash || _lastDash > Time.time)
            return;
        _lastDash = Time.time + data.dashCooldown;
        StartDash();
    }

    private void StartDash()
    {
        var direction = _isLookingRight ? 1 : -1;
        _rb.velocity = Vector2.zero;
        HandleDashConditions(true);
        OnDashUpdate?.Invoke(true);
        StartCoroutine(DashCoroutinePhysics(direction));
    }


    private IEnumerator DashCoroutinePhysics(float dirToMoveX)
    {
        _rb.velocity = new Vector2(data.dashInitialForce * dirToMoveX, 0);
        yield return new WaitForSeconds(_dashTime);
        _rb.velocity = Vector2.zero;
        OnDashUpdate?.Invoke(false);
        HandleDashConditions(false);
    }

    private void HandleDashConditions(bool isDashing)
    {
        SetGravityEnabled(!isDashing);
        Controller.EnableInput(!isDashing);
        _isDashing = isDashing;
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
        if (!isCrouching && CheckCollisionAbove())
        {
            _isPreventedFromUncrouching = true;
            return;
        }

        _crouchSpeedMultiplier = isCrouching ? data.crouchSpeedMultiplier : 1;
        var size = isCrouching ? data.crouchColliderSizeY : _initialColliderHeight;
        var offset = isCrouching ? data.crouchColliderOffsetY : _initialColliderOffset;
        SetColliderData(size, offset);
        OnCrouchUpdate?.Invoke(isCrouching);
    }

    private bool CheckCollisionAbove()
    {
        return Physics2D.RaycastNonAlloc(transform.position, Vector2.up, _checkCollisionsAbove,
            data.aboveHeadMinimumDistance, data.groundCheckLayerMask) != 0;
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

        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position, position + data.aboveHeadMinimumDistance * Vector3.up);
    }
#endif
}