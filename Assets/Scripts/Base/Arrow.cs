using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] ShootableBase _onwner;
    [SerializeField] float _speedMove = 1.5f;

    private bool _init;
    private int _damage;

    public void Init (ShootableBase Owner)
    {
        _onwner = Owner;
        _damage = Owner.DamageValue;
        _init = true;

        Destroy(gameObject, 25f);
    }

    private void Update()
    {
        if (_init)
        {
            transform.position += transform.forward * _speedMove * Time.deltaTime;
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
            {
                if (hit.collider != null && (_onwner == null || hit.collider.transform.root.gameObject != _onwner.gameObject))
                {
                    if (hit.collider.transform.root.GetComponent<IDamageReciever>() != null)
                    {
                        HandleTarget(hit.collider.transform.root.GetComponent<IDamageReciever>());
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void HandleTarget (IDamageReciever damageReciever)
    {
        damageReciever.Damage(_damage);
    }
}