using UnityEngine;
using UnityEngine.InputSystem;

public class RayController : MonoBehaviour
{
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _impactForce = 200f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private Transform _firePoint; 
    [SerializeField] private LayerMask _hitLayers;

    private void Awake()
    {
        if (_firePoint == null) _firePoint = transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current != null && !Mouse.current.enabled)
        {
            InputSystem.EnableDevice(Mouse.current);
        }
#endif
        if (Mouse.current == null) return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Debug.DrawRay(_firePoint.position, -_firePoint.forward * _range, Color.red, 0.5f);

        if(Physics.Raycast(_firePoint.position, -_firePoint.forward, out hit, _range, _hitLayers))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                Vector3 forceDirection = -_firePoint.forward;
                rb.AddForceAtPosition(forceDirection * 0.25f, hit.point, ForceMode.Impulse);
            }

            Target target = hit.collider.GetComponent<Target>();
            DamageEffect effect = hit.collider.GetComponentInParent<DamageEffect>();

            if (effect != null) effect.ShowDamageEffect();

            if (target != null) target.TakeDamage(_damage);

            
        }
    }
}
