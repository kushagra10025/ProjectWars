using System;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingCreator : MonoBehaviour
{
    [SerializeField] private Vector2Reference _mousePosition;
    [SerializeField] private Tilemap _previewTilemap;
    [SerializeField] private Tilemap _defaultTilemap;
    
    private BuildingObjectBase _selectedBuildingObject;
    private Camera _camera;
    private TileBase _tileBase;

    private bool _isOverUI; // TODO Try to replace with Bool Reference
    private Vector3Int _currentGridPosition;
    private Vector3Int _lastGridPosition;
    private Vector3Int _holdStartGridPosition;
    private BoundsInt _dragRectangleBounds;

    private bool _leftMouseHoldAction;

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
                if (_leftMouseHoldAction)
                {
                    HandleDrawing();
                }
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
        if (_selectedBuildingObject != null)
        {
            // Handle Hold Start Grid Position on First Left Click
            _holdStartGridPosition = _currentGridPosition;
            HandleDrawing();
        }
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

    public void OnLeftMouseHoldActionEvent(bool value)
    {
        if (_selectedBuildingObject != null &&
            _selectedBuildingObject.PlaceType == EPlaceType.Single)
        {
            return;
        }
        
        _leftMouseHoldAction = value;
        if (value)
        {
            HandleDrawing();
        }
        else
        {
            HandleReleaseDraw();
        }
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

    private void HandleReleaseDraw()
    {
        if (_selectedBuildingObject != null)
        {
            switch (_selectedBuildingObject.PlaceType)
            {
                case EPlaceType.Line:
                    break;
                case EPlaceType.Rectangle:
                    DrawBounds(_defaultTilemap);
                    _previewTilemap.ClearAllTiles();
                    break;
            }
        }
    }

    private void HandleDrawing()
    {
        if (_selectedBuildingObject != null)
        {
            switch (_selectedBuildingObject.PlaceType)
            {
                case EPlaceType.Single: 
                    DrawTile();
                    break;
                case EPlaceType.Line:
                    break;
                case EPlaceType.Rectangle:
                    RectangleRenderer();
                    break;
                // default:
            }
        }
    }

    private void RectangleRenderer()
    {
        _previewTilemap.ClearAllTiles();
        
        _dragRectangleBounds.xMin = Mathf.Min(_currentGridPosition.x,_holdStartGridPosition.x);
        _dragRectangleBounds.xMax = Mathf.Max(_currentGridPosition.x,_holdStartGridPosition.x);
        _dragRectangleBounds.yMin = Mathf.Min(_currentGridPosition.y,_holdStartGridPosition.y);
        _dragRectangleBounds.yMax = Mathf.Max(_currentGridPosition.y,_holdStartGridPosition.y);
        
        DrawBounds(_previewTilemap);
    }

    private void DrawBounds(Tilemap map)
    {
        for (int x = _dragRectangleBounds.xMin; x <= _dragRectangleBounds.xMax; x++)
        {
            for (int y = _dragRectangleBounds.yMin; y <= _dragRectangleBounds.yMax; y++)
            {
                map.SetTile(new Vector3Int(x, y,0), _tileBase);
            }
        }
    }

    private void DrawTile()
    {
        _defaultTilemap.SetTile(_currentGridPosition, _tileBase);
    }
}
