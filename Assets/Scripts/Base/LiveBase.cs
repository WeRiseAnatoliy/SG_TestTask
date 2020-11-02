using UnityEngine;
using UnityEngine.Events;

public abstract class LiveBase : ShootableBase, IDamageReciever
{
    public int Health => _health;
    public UnityAction<int> OnDamage;
    public UnityAction<IDamageReciever> OnDeath;

    [SerializeField] int _health = 100;
    [SerializeField] LayerMask _rayMask;

    public void Damage(int damage)
    {
        _health -= damage;
        OnDamage?.Invoke(damage);
        if (_health <= 0)
        {
            OnDeath?.Invoke(this);
        }
    }

    protected bool _isVisisble (GameObject target)
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, 90f, _rayMask))
        {
            return hit.collider != null && hit.collider.gameObject.transform.root.gameObject == target;
        }
        return false;
    }

    public bool IsVisibleTarget (GameObject target)
    {
        return _isVisisble(target);
    }
}