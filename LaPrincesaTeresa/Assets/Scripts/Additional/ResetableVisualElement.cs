using Interface;
using UnityEngine;

public class ResetableVisualElement : MonoBehaviour, ILevelResetable
{
    private Vector3 _startingPos;
    private Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _startingPos = transform.position;
        TryGetComponent(out _rb);
    }

    public void OnResetLevel()
    {
        transform.position = _startingPos;
        if (_rb != default)
        {
            _rb.velocity = Vector3.zero;
        }
    }
}