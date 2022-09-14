using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float parallaxEffectX;

    // [SerializeField, Range(0, 1)] private float parallaxEffectY;
    private float _lenght;
    private float _startPosX;
  //  private float _startPosY;
    private Transform _cam;

    private void Awake()
    {
        _startPosX = transform.position.x;
        _lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        if (Camera.main != null)
            _cam = Camera.main.transform;
        // _startPosY = transform.position.y;
    }

    private void FixedUpdate()
    {
        var position = _cam.position;
        var temp = (position.x * (1 - parallaxEffectX));

        var distance = (position.x * parallaxEffectX);
        // var distance2 = (position.y * parallaxEffectY);

        var transform1 = transform;
        var position1 = transform1.position;
        position1 = new Vector3(_startPosX + distance, position1.y, position1.z);
        // position1 = new Vector3(position1.x, _startPosY + distance2, position1.z);
        transform1.position = position1;

        if (temp > _startPosX + _lenght)
        {
            _startPosX += _lenght;
            return;
        }

        if (temp < _startPosX - _lenght)
        {
            _startPosX -= _lenght;
        }
    }
}