using UnityEngine;
using UnityEngine.Tilemaps;

public enum EPlaceType
{
    None = 0,
    Single = 1,
    LineHorizontal = 2,
    LineVertical = 3,
    LineFreeform = 4,
    LineDiagonal = 5,
    Rectangle = 6
}

[CreateAssetMenu(fileName = "New Buildable Category", menuName = "Wars/New Buildable Category", order = 2)]
public class BuildingCategory : ScriptableObject
{
    [SerializeField] private EPlaceType _placeType;
    [SerializeField] private int _sortingOrder;

    private Tilemap _tilemap;
    
    public EPlaceType PlaceType => _placeType;
    public int SortingOrder => _sortingOrder;

    public Tilemap Tilemap
    {
        get => _tilemap;
        set => _tilemap = value;
    }
}
