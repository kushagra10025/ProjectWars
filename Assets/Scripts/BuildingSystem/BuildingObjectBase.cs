using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Buildable", menuName = "Wars/New Buildable", order = 1)]
public class BuildingObjectBase : ScriptableObject
{
    [SerializeField] private BuildingCategory _category;
    [SerializeField] private TileBase _tileBase;
    [SerializeField] private EPlaceType _placeType;

    public BuildingCategory Category => _category;
    public TileBase TileBase => _tileBase;
    public EPlaceType PlaceType => _placeType == EPlaceType.None ? _category.PlaceType : _placeType;
}
