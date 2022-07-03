using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeedMultiplier;
    public float jumpInitialForce;
    public float jumpContinousForce;
    public float maxJumpTime;
    public float maxFallSpeed;
    [Header("GroundCheck")] public float groundCheckAreaRadius;
    public LayerMask groundCheckLayerMask;
    public float coyoteTime;
    [Header("Interaction")] 
    public LayerMask interactionLayers;
    public float interactionRadius;
    

}