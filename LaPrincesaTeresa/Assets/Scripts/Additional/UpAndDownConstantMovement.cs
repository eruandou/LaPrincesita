using System.Collections;
using ScriptableObjects.Extras;
using UnityEngine;

public class UpAndDownConstantMovement
{
    private ItemData _data;
    private float _sinCenterY;
    private float _currentSpeed;
    private Transform _visual;

    public UpAndDownConstantMovement(Transform visualTransform, ItemData data)
    {
        _visual = visualTransform;
        _sinCenterY = _visual.localPosition.y;
        _data = data;
        _currentSpeed = _data.unpickedSpeed;
    }

    public void SpeedChange()
    {
        _currentSpeed = _currentSpeed == _data.pickedSpeed ? _data.unpickedSpeed : _data.pickedSpeed;
    }

    public IEnumerator UpAndDownMovement()
    {
        while (true)
        {
            Vector2 pos = _visual.localPosition;
            pos.y = Mathf.Sin(Time.time) * 0.25f + _sinCenterY * Time.deltaTime * _currentSpeed;
            _visual.localPosition = pos;
            yield return null;
        }
    }
}