using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : NavigableBase
{
    [SerializeField] Transform[] _navPoints;

    private PlayerController _playerController;
    private byte _currentPoint;
    private Coroutine _shootingCoroutine;

    public void Init (PlayerController playerController)
    {
        _playerController = playerController;
        OnDeath += Death;
    }

    private void Update()
    {
        if(_navPoints.Length > 0)
        {
            if(_canShoot)
            {
                if (_shootingCoroutine == null)
                {
                    _shootingCoroutine = StartCoroutine(_shooting());
                }
            }
            else
            {
                Vector3 direction = _navPoints[_currentPoint].position - transform.position;
                _move(direction);

                float distance = Vector3.Distance(_navPoints[_currentPoint].position, transform.position);
                if(distance < 1f)
                {
                    _currentPoint++;
                    if(_currentPoint > _navPoints.Length - 1)
                    {
                        _currentPoint = 0;
                    }
                }
            }
        }
        else
        {
            if (_canShoot)
            {
                if (_shootingCoroutine == null)
                {
                    _shootingCoroutine = StartCoroutine(_shooting());
                }
            }
        }
    }

    private IEnumerator _shooting()
    {
        Vector3 direction = _playerController.transform.position - transform.position;
        _lookAtPoint(direction);
        yield return new WaitForSeconds(0.3f);
        if (_canShoot)
            Shoot(direction);

        _shootingCoroutine = null;
    }

    private void Death (IDamageReciever damageReciever)
    {
        Destroy(gameObject);
    }
}
