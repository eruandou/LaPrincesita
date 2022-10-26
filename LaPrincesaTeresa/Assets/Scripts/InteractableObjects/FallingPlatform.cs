using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float countDown;
    [SerializeField] private GameObject visual;

    private Vector3 _origin;
    private float _currCd;
    private float _destroyTime;
    private Rigidbody2D _rb;
    private Collider2D[] _collider2Ds;
    private bool _hasToFall;
    private bool _restore;
    private bool _activeTrap;
    private Animator _animator;
    private void Awake()
    {
        _hasToFall = true;
        _restore = false;
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

        if (_currCd < Time.time && _hasToFall)
        {
            _rb.isKinematic = false;
            _hasToFall = false;
            _destroyTime = Time.time + countDown;
        }

        if (!_hasToFall && _destroyTime < Time.time)
        {
            EnableDisableThingies(false);
        }

        if (_restore)
        {
            EnableDisableThingies(true);
            ReproduceAnimation(false);
            _hasToFall = true;
            _activeTrap = false;
            _rb.isKinematic = true;
            _rb.velocity = Vector2.zero;
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
            _restore = false;
        }
        else
        {
            _restore = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        _currCd = Time.time + timer;
        ReproduceAnimation(true);
        _activeTrap = true;
    }
    private void ReproduceAnimation(bool state)
    {
        _animator.SetBool("Tembleque", state);

    }
}