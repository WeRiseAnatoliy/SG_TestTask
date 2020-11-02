using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _smoothing = 1f;
    [SerializeField] Vector3 _offset = Vector3.up;
    [SerializeField] Vector2 _minMaxX;
    [SerializeField] Vector2 _minMaxZ;
    [SerializeField] bool _debug;

    private PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _alignToTarget(false);
    }

    private void Update()
    {
        if(_player != null)
        {
            _alignToTarget(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(_minMaxX.x, 0, _minMaxZ.x), new Vector3(_minMaxX.x, 0, _minMaxZ.y));
            Gizmos.DrawLine(new Vector3(_minMaxX.y, 0, _minMaxZ.x), new Vector3(_minMaxX.y, 0, _minMaxZ.y));
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(_minMaxX.x, 0, _minMaxZ.x), new Vector3(_minMaxX.y, 0, _minMaxZ.x));
            Gizmos.DrawLine(new Vector3(_minMaxX.x, 0, _minMaxZ.y), new Vector3(_minMaxX.y, 0, _minMaxZ.y));
        }
    }

    private void _alignToTarget (bool Smooth)
    {
        Vector2 vertBorders = _getCameraVerticalBorder();
        Vector2 horizBorders = _getCameraHorizontalBorder();
        Vector3 targetPosition = _getTargetPosition();
        targetPosition.x = Mathf.Clamp(targetPosition.x, _minMaxX.x + horizBorders.y, _minMaxX.y - horizBorders.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, _minMaxZ.x, _minMaxZ.y);

        transform.position = Smooth ? Vector3.Lerp(transform.position, targetPosition, _smoothing * Time.deltaTime) : targetPosition;
    }

    private Vector3 _getTargetPosition ()
    {
        return _player != null ? _player.transform.position + _offset : transform.position;
    }

    private Vector2 _getCameraHorizontalBorder()
    {
        Camera m_camera = Camera.main;

        Vector3 leftPos = m_camera.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.5f));
        Vector3 rightPos = m_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height * 0.5f));

        float l = Mathf.Abs(transform.position.x - leftPos.x);
        float r = Mathf.Abs(transform.position.x - rightPos.x);

        return new Vector2(r, l);
    }

    private Vector2 _getCameraVerticalBorder()
    {
        Camera m_camera = Camera.main;

        Vector3 upPos = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height));
        Vector3 downPos = m_camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, 0f));

        float u = Mathf.Abs(transform.position.z - upPos.y);
        float d = Mathf.Abs(transform.position.z - downPos.y);

        return new Vector2(u, d);
    }
}