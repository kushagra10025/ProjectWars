using System;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingCreator : MonoBehaviour
{
    [SerializeField] private Vector2Reference _mousePosition;
    [SerializeField] private Tilemap _previewTilemap;
    
    private BuildingObjectBase _selectedBuildingObject;
    private Camera _camera;
    private TileBase _tileBase;

    private bool _isOverUI; // TODO Try to replace with Bool Reference
    private Vector3Int _currentGridPosition;
    private Vector3Int _lastGridPosition;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        _isOverUI = EventSystem.current.IsPointerOverGameObject();

        if (_selectedBuildingObject != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(_mousePosition.Value);
            Vector3Int gridPos = _previewTilemap.WorldToCell(pos);
            if (gridPos != _currentGridPosition)
            {
                _lastGridPosition = _currentGridPosition;
                _currentGridPosition = gridPos;
                
                UpdatePreview();
            }
        }
    }

    public void OnSelectedBuildingObjectChanged(UnityEngine.Object obj)
    {
        if (!(obj is BuildingObjectBase && obj != null)) return;
        _selectedBuildingObject = (BuildingObjectBase) obj;
        _tileBase = _selectedBuildingObject.TileBase;
        // Debug.Log(_selectedBuildingObject.name);
        UpdatePreview();
    }

    public void OnLeftMouseActionEvent(bool value)
    {
        if (_isOverUI) return;
        Debug.Log("Left Mouse : " + value);
    }

    public void OnRightMouseActionEvent(bool value)
    {
        // If Over UI Don't do anything on Right Click - Unless new functionality decided
        if (_isOverUI) return;
        
        // If Right Clicked, Remove the Preview TileBase and UpdatePreview
        // Try to check with OnSelectedBuildingObjectChanged
        _tileBase = null;
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        // Generic TileBase for all tile preview. Will save lot of work.
        // Clear Last Grid Position
        _previewTilemap.SetTile(_lastGridPosition, null);
        // Update Current Position with currently selected TileBase
        // If mouse is over UI set current position TileBase to null
        _previewTilemap.SetTile(_currentGridPosition, _isOverUI ? null : _tileBase);
    }
}
