using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public void SubscribeToEvents(PlayerModel model)
    {
        model.OnCrouchUpdate += OnCrouchHandler;
        model.OnJumpUpdate += OnJumpHandler;
        model.OnMoveUpdate += OnMoveHandler;
    }

    private void OnMoveHandler(float moveAmount)
    {
#if UNITY_EDITOR
        print($"Move amount {moveAmount}");
#endif
    }

    private void OnJumpHandler(bool isJumping)
    {
#if UNITY_EDITOR
        print($"Is jumping: {isJumping}");
#endif
    }

    private void OnCrouchHandler(bool isCrouched)
    {
#if UNITY_EDITOR
        print($"Is crouching: {isCrouched}");
#endif
    }
}