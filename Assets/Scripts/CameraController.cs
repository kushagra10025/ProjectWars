using System;
using Cinemachine;
using ScriptableObjectArchitecture;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Vector2Reference _moveInput;

    private Vector2 m_screenBounds;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineConfiner2D _confiner2D;
    private Bounds _confinerBounds;
    
    private void Awake()
    {
        if(_camera != null)
        {
            _virtualCamera = _camera.GetComponent<CinemachineVirtualCamera>();
            _confiner2D = _camera.GetComponent<CinemachineConfiner2D>();
            _confinerBounds = _confiner2D.m_BoundingShape2D.bounds;
        }
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 transformCache = transform.position;
        
        transformCache.x += _moveInput.Value.x * Time.deltaTime * _movementSpeed;
        transformCache.y += _moveInput.Value.y * Time.deltaTime * _movementSpeed;

        transformCache.x = Mathf.Clamp(transformCache.x, _confinerBounds.min.x, _confinerBounds.max.x);
        transformCache.y = Mathf.Clamp(transformCache.y, _confinerBounds.min.y, _confinerBounds.max.y);
        
        transform.position = transformCache;
    }

    private void OnDrawGizmosSelected()
    {
        Bounds bounds = _camera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D.bounds;
        // Bottom Extent
        Debug.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), Color.blue);
        // Right Extent
        Debug.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), Color.blue);
        // Top Extent
        Debug.DrawLine(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), Color.blue);
        // Left Extent
        Debug.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), new Vector3(bounds.min.x, bounds.min.y, bounds.min.z), Color.blue);
        // Diagonal Extent
        // Debug.DrawLine(bounds.min, bounds.max, Color.blue);
    }
}
