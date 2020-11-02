using System.Collections;
using UnityEngine;

public abstract class ShootableBase : MonoBehaviour
{
    [SerializeField] private Arrow _arrowPrefab;
    [SerializeField] private Transform _outputPoint;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] protected bool _canShoot = true;

    public int DamageValue = 5;

    private IEnumerator _fireCoroutine ()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_fireRate);
        _canShoot = true;
    }

    protected void Shoot (Vector3 direction)
    {
        Arrow arrow = Instantiate(_arrowPrefab, _outputPoint.position, Quaternion.LookRotation(direction));
        arrow.Init(this);
        StartCoroutine(_fireCoroutine());
    }
}