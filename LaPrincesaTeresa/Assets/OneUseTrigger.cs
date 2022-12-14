using UnityEngine;
using UnityEngine.Events;

public class OneUseTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask layerToCheckAgainst;
    [SerializeField] protected UnityEvent<PlayerModel> onPickUp;

    private void Awake()
    {
        onPickUp.AddListener((model)=>DestroyTriggerer());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStaticFunctions.IsGoInLayerMask(col.gameObject, layerToCheckAgainst) &&
            col.TryGetComponent(out PlayerModel model))
        {
            onPickUp.Invoke(model);
        }
    }

    public void DestroyTriggerer()
    {
        Destroy(gameObject);
    }
}