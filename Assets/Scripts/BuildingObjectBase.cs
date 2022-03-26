using UnityEngine;
using UnityEngine.Tilemaps;

public enum ECategory
{
    Ground,
    Obstacle,
    Road
}

[CreateAssetMenu(fileName = "New Buildable", menuName = "Wars/New Buildable", order = 1)]
public class BuildingObjectBase : ScriptableObject
{
    [SerializeField] private ECategory _category;
    [SerializeField] private TileBase _tileBase;

    public ECategory Category => _category;
    public TileBase TileBase => _tileBase;
}
