using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    private List<EnemyController> _enemys;
    private PlayerController _playerController;

    public UnityAction OnLevelComplete;
    public UnityAction OnLevelLoose;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        if(_playerController != null)
        {
            _playerController.OnDeath += OnPlayerDeath;
        }
        _enemys = new List<EnemyController>(FindObjectsOfType<EnemyController>());
        foreach(EnemyController enemy in _enemys)
        {
            enemy.Init(_playerController);
            enemy.OnDeath += OnEnemyDeath;
        }
    }

    private void OnPlayerDeath (IDamageReciever reciever)
    {

    } 

    private void OnEnemyDeath (IDamageReciever reciever)
    {
        _enemys.Remove((EnemyController)reciever);
        if(_enemys.Count == 0)
        {
            OnLevelComplete?.Invoke();
        }
    }

    public bool HaveEnemys ()
    {
        return _enemys != null && _enemys.Count > 0;
    }

    public EnemyController GetBestEnemy ()
    {
        if (_enemys == null || _enemys.Count == 0) return null;
        else if (_enemys.Count == 1) return _enemys[0];

        float best = float.MaxValue;
        EnemyController bestEnemy = null;

        foreach (EnemyController enemy in _enemys)
        {
            if(_playerController.IsVisibleTarget(enemy.gameObject))
            {
                if (bestEnemy == null)
                {
                    bestEnemy = enemy;
                    best = Vector3.Distance(_playerController.transform.position, enemy.transform.position);
                }
                else
                {
                    float currentDistance = Vector3.Distance(_playerController.transform.position, enemy.transform.position);
                    if (currentDistance < best)
                    {
                        best = currentDistance;
                        bestEnemy = enemy;
                    }
                }
            }
        }

        if (bestEnemy != null)
        {
            return bestEnemy;
        }

        best = float.MaxValue;
        
        foreach(EnemyController enemy in _enemys)
        {
            if(bestEnemy == null)
            {
                bestEnemy = enemy;
                best = Vector3.Distance(_playerController.transform.position, enemy.transform.position);
            }
            else
            {
                float currentDistance = Vector3.Distance(_playerController.transform.position, enemy.transform.position);
                if (currentDistance < best) {
                    best = currentDistance;
                    bestEnemy = enemy;
                }
            }
        }

        return bestEnemy;
    }
}