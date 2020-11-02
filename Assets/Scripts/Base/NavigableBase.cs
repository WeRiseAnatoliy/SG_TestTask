using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NavigableBase : LiveBase
{
    [SerializeField]
    protected float _speedMove = 3f;
    protected Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void _move (Vector3 direction)
    {
        Vector3 resultDirection = Vector3.forward * direction.z + Vector3.right * direction.x;

        _lookAtPoint(resultDirection);
        _rigidbody.velocity = resultDirection * _speedMove;
    }

    protected virtual void _lookAtPoint (Vector3 directionPoint)
    {
        transform.rotation = Quaternion.LookRotation(directionPoint);
    }
}