using System;
using System.Collections;
using Interface;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    private float _currentSpeed;

    private bool _hasDoubleJump;
    private bool _isJumping;
    private float _crouchSpeedMultiplier;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private Transform groundCheckPoint;

    public event Action<float> OnMoveUpdate;
    public event Action<bool> OnJumpUpdate;
    public event Action<bool> OnCrouchUpdate;

    private float _moveDirCached;

    private Rigidbody2D _rb;

    private bool _isLookingRight;

    private Collider2D[] _interactablesArray = new Collider2D[2];
    private Collider2D[] _groundedArray = new Collider2D[2];

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
        _moveDirCached += moveDir;
    }

    private void FixedUpdate()
    {
        var currentSpeed = _currentSpeed * _crouchSpeedMultiplier;
        OnMoveUpdate?.Invoke(_moveDirCached * _currentSpeed);
        
        print($"Current speed {currentSpeed}");

        if (_moveDirCached == 0)
            return;
        
        _rb.velocity = new Vector2(currentSpeed * _moveDirCached * Time.fixedDeltaTime, _rb.velocity.y);
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
        return Physics2D.OverlapCircle(groundCheckPoint.position, data.groundCheckAreaRadius, data.groundCheckLayerMask);
    }
    private void JumpHandler(bool jumpState)
    {
        print("Jump");
        var isGrounded = CheckGrounded();
        switch (jumpState)
        {
            case true when isGrounded:
                StartJump();
                return;
            case false when !isGrounded:
                EndJump();
                break;
        }
    }

    private void StartJump()
    {
        print("Start jump");
        _rb.AddForce(Vector2.up * data.jumpInitialForce, ForceMode2D.Impulse);
    }

    private void EndJump()
    {
        print("End jump");
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


    private void CrouchHandler(bool obj)
    {
        _crouchSpeedMultiplier = obj ? data.crouchSpeedMultiplier : 1;
        OnCrouchUpdate?.Invoke(obj);
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null || groundCheckPoint == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPoint.position, data.interactionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, data.groundCheckAreaRadius);
    }
#endif
}