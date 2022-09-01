using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerView : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Transform visuals;
    [SerializeField] private float doubleJumpSpinDuration;
    private bool _isCrouching, _isJumping, _isGrounded, _isGliding, _isDashing;
    private static readonly int Movement = Animator.StringToHash("Movement");
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
    }

    private void SetDashing(bool isDashing)
    {
        _isDashing = isDashing;
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
        StartCoroutine(DoubleJumpSpin(doubleJumpSpinDuration));
    }

    private IEnumerator DoubleJumpSpin(float duration)
    {
        var startRotation = transform.eulerAngles.y;
        var endRotation = startRotation + (startRotation > 360 ? 360 : -360);
        var currentTime = 0.0f;
        var transform1 = visuals.transform;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            var yRotation = Mathf.Lerp(startRotation, endRotation, currentTime / duration) % 360.0f;
            var eulerAngles = transform1.eulerAngles;
            eulerAngles.y = yRotation;
            transform1.eulerAngles = eulerAngles;
            yield return null;
        }

        transform1.eulerAngles = Vector3.zero;
    }

    private void EvaluateAnimation()
    {
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