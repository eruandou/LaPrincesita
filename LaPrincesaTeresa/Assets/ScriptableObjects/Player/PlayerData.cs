using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    [Range(0,1)] public float crouchSpeedMultiplier;
    public float crouchColliderSizeY, crouchColliderOffsetY;
    public float jumpInitialForce;
    [Tooltip("This represents the players usual gravity for most situations")]
    public float normalGravityValue = -9.81f;
    [Tooltip("This represents the players gravity specifically for falling")]
    public float maxFallGravityValue = -22f;
    [Tooltip("The max fall speed for the player")]
    public float maxFallSpeed = -30f;
    [Header("GroundCheck")] public float groundCheckAreaRadius;
    public LayerMask groundCheckLayerMask;
    public Vector2 groundCheckBoxSize;
    public float coyoteTime;
    [Header("Interaction")] 
    public LayerMask interactionLayers;
    public float interactionRadius;
    

}