using System;
using System.Linq;
using Interface;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KillZone : MonoBehaviour
{
    private ILevelResetable[] _allResetables;

    private void Awake()
    {
        _allResetables = FindObjectsOfType<MonoBehaviour>().OfType<ILevelResetable>().ToArray();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out PlayerModel model)) return;

        model.OnPlayerKilled();
        ResetAllElements();
    }

#if UNITY_EDITOR
    [ContextMenu("Validate killzone")]
    private void ValidateKillZone()
    {
        if (TryGetComponent(out Collider2D col))
        {
            col.isTrigger = true;
        }

        gameObject.layer = LayerMask.NameToLayer("Triggerer");
    }
#endif

    public void ResetLevel()
    {
        ResetAllElements();
    }

    private void ResetAllElements()
    {
        for (int i = 0; i < _allResetables.Length; i++)
        {
            _allResetables[i].OnResetLevel();
        }
    }
}