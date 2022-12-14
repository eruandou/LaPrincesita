using UnityEngine;
using UnityEngine.Serialization;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float countDown;
    [SerializeField] private GameObject visual;
    [SerializeField] private bool hasToFall = true;
    [SerializeField] private bool restore = false;
    [SerializeField] private LayerMask contactLayers;
    private Vector3 _origin;
    private float _currCd;
    private float _destroyTime;
    private Rigidbody2D _rb;
    private Collider2D[] _collider2Ds;
    private bool _activeTrap;
    private Animator _animator;
    private static readonly int Tembleque = Animator.StringToHash("Tembleque");

    private void Awake()
    {
        _activeTrap = false;

        _origin = transform.position;
        _collider2Ds = GetComponents<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rb.isKinematic = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (!_activeTrap) return;

        if (_currCd < Time.time && hasToFall)
        {
            _rb.isKinematic = false;
            hasToFall = false;
            _destroyTime = Time.time + countDown;
        }

        if (!hasToFall && _destroyTime < Time.time)
        {
            EnableDisableThingies(false);
        }
    }

    private void EnableDisableThingies(bool enaDis)
    {
        for (int i = 0; i < _collider2Ds.Length; i++)
        {
            _collider2Ds[i].enabled = enaDis;
        }

        visual.SetActive(enaDis);
        if (enaDis)
        {
            transform.position = _origin;
        }
        else
        {
            Restore();
        }
    }

    void Restore()
    {
        EnableDisableThingies(true);
        ReproduceAnimation(false);
        hasToFall = true;
        _activeTrap = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameStaticFunctions.IsGoInLayerMask(other.gameObject, contactLayers))
            _currCd = Time.time + timer;
        ReproduceAnimation(true);
        _activeTrap = true;
    }

    private void ReproduceAnimation(bool state)
    {
        _animator.SetBool(Tembleque, state);
    }
}