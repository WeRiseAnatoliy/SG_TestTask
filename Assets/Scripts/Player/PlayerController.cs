using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NavigableBase
{
    [SerializeField] FixedJoystick _joystick;

    private LevelController _levelController;
    private GameObject _enemy;

    private void Start()
    {
        _levelController = FindObjectOfType<LevelController>();
        ReloadEnemy();
    }

    private void Update()
    {
        if(_joystick.IsPressed())
        {
            _move(_joystick.MoveInput());
        }
        else
        {
            if(_canShoot)
            {
                if (_levelController.HaveEnemys())
                {
                    ReloadEnemy();
                }

                if (_enemy != null)
                {
                    Vector3 direction = _enemy.transform.position - transform.position;
                    _lookAtPoint(direction);
                    Shoot(direction);
                }
            }
        }
    }

    private void ReloadEnemy ()
    {
        EnemyController bestEnemy = _levelController.GetBestEnemy();
        _enemy = bestEnemy != null ? bestEnemy.gameObject : null;
    }
}