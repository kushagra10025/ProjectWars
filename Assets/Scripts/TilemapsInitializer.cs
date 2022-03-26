using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsInitializer : MonoBehaviourSingleton<TilemapsInitializer>
{
    [SerializeField] private List<BuildingCategory> _categoriesToCreateTilemap;
    [SerializeField] private Transform _gridTransform;

    private void Start()
    {
        CreateMaps();
    }

    private void CreateMaps()
    {
        foreach (BuildingCategory _category in _categoriesToCreateTilemap)    
        {
            // Create Game Object
            GameObject go = new GameObject("Tilemap_" + _category.name);
            // Create Tilemap Rectangular Component
            Tilemap map = go.AddComponent<Tilemap>();
            TilemapRenderer mapRenderer = go.AddComponent<TilemapRenderer>();
            // Set Parent
            go.transform.SetParent(_gridTransform);
            // Apply Tilemap Components Settings
            mapRenderer.sortingOrder = _category.SortingOrder;
            _category.Tilemap = map;
        }
    }
}
