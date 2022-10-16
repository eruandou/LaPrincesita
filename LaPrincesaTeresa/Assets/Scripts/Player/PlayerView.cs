using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerView : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private float doubleJumpSpinDuration;
    private bool _isCrouching, _isJumping, _isGrounded, _isGliding, _isDashing, _isDead;
    private static readonly int Movement = Animator.StringToHash("Movement");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private WaitForSeconds _waitTimeForSpinAnim;
    private int _spinJumpLayer;
    private int _deadLayer;

    public static event Action OnStartJumpFromGround = delegate { };

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spinJumpLayer = _animator.GetLayerIndex("Twirl");
        _deadLayer = _animator.GetLayerIndex("DeadLayer");
        _waitTimeForSpinAnim = new WaitForSeconds(doubleJumpSpinDuration);
    }

    public void SubscribeToEvents(PlayerModel model)
    {
        model.OnCrouchUpdate += OnCrouchHandler;
        model.OnJumpUpdate += OnJumpHandler;
        model.OnMoveUpdate += OnMoveHandler;
        model.OnGroundedUpdate += SetGrounded;
        model.OnGlidingUpdate += SetGliding;
        model.OnDashUpdate += SetDashing;
        model.OnDoubleJump += DoubleJump;
        model.OnDieUpdate += SetDead;
    }

    private void SetDashing(bool isDashing)
    {
        _isDashing = isDashing;
    }

    private void SetDead(bool isDead)
    {
        _isDead = isDead;
        StartCoroutine(PlayerIsDead());
    }

    private void SetGliding(bool isGliding)
    {
        _isGliding = isGliding;
    }

    private void OnMoveHandler(float moveAmount)
    {
        _animator.SetFloat(Movement, moveAmount);
    }

    private void OnJumpHandler(bool isJumping)
    {
        if (isJumping && _isGrounded)
        {
            OnStartJumpFromGround();
        }

        _isJumping = isJumping;
        _animator.SetBool(Jumping, isJumping);
    }

    private void SetGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }

    private void OnCrouchHandler(bool isCrouched)
    {
        _isCrouching = isCrouched;
    }

    private void Update()
    {
        EvaluateAnimation();
    }

    private void DoubleJump()
    {
        StartCoroutine(DoubleJumpSpin());
    }

    private IEnumerator PlayerIsDead()
    {
        _animator.SetLayerWeight(_deadLayer, 1);
        _animator.Play("DeadBreak", _deadLayer, 0);

        yield return _waitTimeForSpinAnim;
        _animator.SetLayerWeight(_deadLayer, 0);
    }

    private IEnumerator DoubleJumpSpin()
    {
        _animator.SetLayerWeight(_spinJumpLayer, 1);
        _animator.Play("Twirl", _spinJumpLayer, 0);

        yield return _waitTimeForSpinAnim;
        _animator.SetLayerWeight(_spinJumpLayer, 0);
    }

    private void EvaluateAnimation()
    {
        if (_isDead)
        {
            _animator.Play("PlayerDead");
            return;
        }

        if (_isCrouching)
        {
            _animator.Play("Crouched");
            return;
        }

        if (_isDashing)
        {
            _animator.Play("Dashing");
            return;
        }

        if (_isGrounded)
        {
            _animator.Play("MovementBlendTree");
            return;
        }

        if (_isJumping)
        {
            _animator.Play("Jumping");
            return;
        }

        if (_isGliding)
        {
            _animator.Play("Gliding");
            return;
        }

        _animator.Play("Falling");
    }
}