using Managers;
using UnityEngine;

public class OneUsePowerUp : OneUseTrigger
{
    [SerializeField] private PowerupType powerUpToGive;

    private bool AlreadyCollected()
    {
        var saveData = GameManager.Instance.DataSaver.GetCurrentSaveData();

        return powerUpToGive switch
        {
            PowerupType.DoubleJump => saveData.doubleJump,
            PowerupType.Dash => saveData.dash,
            PowerupType.Glide => saveData.dash,
            _ => false
        };
    }

    private void Start()
    {
        if (AlreadyCollected())
        {
            DestroyTriggerer();
            return;
        }

        onPickUp.AddListener(HandlePickupPowerUp);
    }

    private void HandlePickupPowerUp(PlayerModel model)
    {
        GameManager.Instance.PowerUpGet(powerUpToGive);
        model.ReloadSavedData();
    }
}