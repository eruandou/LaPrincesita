using System;
using Interface;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public event Action<float> OnMoveUpdate;
    public event Action<bool> OnJumpUpdate;
    public event Action<bool> OnCrouchUpdate;

    [SerializeField] private PlayerData data;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private Transform groundCheckPoint;

    private bool _isJumping, _hasDoubleJump, _isLookingRight, _isGrounded;
    private float _currentSpeed, _crouchSpeedMultiplier, _currentGravity, _moveDirCached;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb;
    private Collider2D[] _interactablesArray = new Collider2D[2];
    private Collider2D[] _groundedArray = new Collider2D[2];

    #region Attributes
    // All attributes that can be modifypublic int _jump


    private int _jump;
    private float _dash;

    #endregion

    public void SubscribeToController(IPlayerController controller)
    {
        controller.OnCrouch += CrouchHandler;
        controller.OnInteract += InteractHandler;
        controller.OnJump += JumpHandler;
        controller.OnMove += MoveHandler;
        controller.OnRun += RunHandler;
        controller.OnOpenInventory += OpenInventoryHandler;
    }

    private void Awake()
    {
        _currentSpeed = data.walkSpeed;
        _rb = GetComponent<Rigidbody2D>();
        _isLookingRight = true;
        _crouchSpeedMultiplier = 1f;
        _currentGravity = data.normalGravityValue;
        _collider = GetComponent<BoxCollider2D>();
        SetColliderData(1, 0);
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
        if (_rb.velocity.y < data.maxFallSpeed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, data.maxFallSpeed);
            return;
        }

        _rb.AddForce(Vector2.up * (_currentGravity * Time.fixedDeltaTime), ForceMode2D.Force);
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

        _rb.velocity = new Vector2(currentSpeed * _moveDirCached, _rb.velocity.y);
        print("freeeze 2");
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
        if (GameStaticFunctions.IsGoInLayerMask(col.gameObject, data.groundCheckLayerMask))
        {
            _isGrounded = true;
            _currentGravity = data.normalGravityValue;
        }
    }

    private void JumpHandler(bool jumpState)
    {

        _isGrounded = CheckGrounded();
        switch (jumpState)
        {
            case true when _isGrounded:
                StartJump();
                break;
            case false when !_isGrounded:
                EndJump();
                break;
        }
    }

    private void StartJump()
    {
       // print("Aca");
        _rb.AddForce(Vector2.up * data.jumpInitialForce, ForceMode2D.Impulse);
    }

    private void EndJump()
    {
       // print("Aca No");
        _currentGravity = data.maxFallGravityValue;
    }

    public void SetJumpAmount(int times)
    {
        _jump += times;
    }    
    public void SetDashForce(float force)
    {
        _dash = force;
    }

    private void InteractHandler()
    {
        var interactablesFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, data.interactionRadius,
            _interactablesArray, data.interactionLayers);

        if (interactablesFound < 1)
            return;

        var interactable = _interactablesArray[0].GetComponent<IInteractable>();
        interactable?.OnInteract();
    }


    private void CrouchHandler(bool isCrouching)
    {
        _crouchSpeedMultiplier = isCrouching ? data.crouchSpeedMultiplier : 1;
        var size = isCrouching ? data.crouchColliderSizeY : 1;
        var offset = isCrouching ? data.crouchColliderOffsetY : 0;
        SetColliderData(size, offset);
        OnCrouchUpdate?.Invoke(isCrouching);
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