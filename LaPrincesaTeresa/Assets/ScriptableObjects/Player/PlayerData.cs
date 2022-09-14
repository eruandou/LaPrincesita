using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Header("Movement")] public float walkSpeed;
    public float runSpeed;
    [Range(0, 1)] public float crouchSpeedMultiplier;
    public float crouchColliderSizeY, crouchColliderOffsetY;
    [Min(0)] public float jumpInitialForce;
    [Min(0)] public float dashInitialForce;
    [Min(0)] public float dashCooldown = 3f;
    [Min(0)] public float dashTime = 0.8f;
    public float glidingGravity;
    [Min(0.1f)] public float glidingTime;

    [Tooltip("This represents the players usual gravity for most situations")]
    public float jumpGravityValue = -9.81f;

    [Tooltip("This represents the players gravity specifically for falling")]
    public float FallGravity = -15000f;

    [Tooltip("The max fall speed for the player")]
    public float maxFallSpeed = -30f;

    [Header("GroundCheck")] public LayerMask groundCheckLayerMask;
    public Vector2 groundCheckBoxSize;
    [Min(0)] public float coyoteTime;
    [Min(0)] public float aboveHeadMinimumDistance = 1f;
    [Header("Interaction")] public LayerMask interactionLayers;
    [Min(0)] public float interactionRadius;
    public bool instantFalling = true;
}