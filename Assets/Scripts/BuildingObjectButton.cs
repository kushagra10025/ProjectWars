using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingObjectButton : MonoBehaviour
{
    [SerializeField] private BuildingObjectBase _objectBase;
    [SerializeField] private Image _buildTileImage;
    [SerializeField] private ObjectReference _selectedBuildingObject;
    
    public void Start()
    {
        _buildTileImage.sprite = ((Tile) _objectBase.TileBase).sprite;
    }

    public void OnBuildingButtonClicked()
    {
        _selectedBuildingObject.Value = _objectBase;
    }
}
