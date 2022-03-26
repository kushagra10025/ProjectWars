using UnityEngine;
using UnityEngine.Tilemaps;

public enum ECategory
{
    Ground,
    Obstacle,
    Road
}

public enum EPlaceType
{
    Single,
    Line,
    Rectangle
}

[CreateAssetMenu(fileName = "New Buildable", menuName = "Wars/New Buildable", order = 1)]
public class BuildingObjectBase : ScriptableObject
{
    [SerializeField] private ECategory _category;
    [SerializeField] private TileBase _tileBase;
    [SerializeField] private EPlaceType _placeType;

    public ECategory Category => _category;
    public TileBase TileBase => _tileBase;
    public EPlaceType PlaceType => _placeType;
}
