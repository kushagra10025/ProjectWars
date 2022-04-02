using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsInitializer : MonoBehaviourSingleton<TilemapsInitializer>
{
    [SerializeField] private List<BuildingCategory> _categoriesToCreateTilemap;
    [SerializeField] private Transform _gridTransform;

    [SerializeField] private string currentLevelName = "Default_Level";
    [SerializeField] private string savesDirectory = "saves";

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

    public void SaveMap()
    {
        string tilemapsJson = "[";
        for (var index = 0; index < _categoriesToCreateTilemap.Count; index++)
        {
            var _catTilemap = _categoriesToCreateTilemap[index];
            string tilemapParsedData = ParseTilemapLevelData(_catTilemap.Tilemap, _catTilemap.name);
            tilemapsJson += tilemapParsedData;
            if(index != _categoriesToCreateTilemap.Count-1)
                tilemapsJson += ",";
        }

        tilemapsJson += "]";
        File.WriteAllText(Application.dataPath + "/" + savesDirectory + "/" + currentLevelName + ".json",tilemapsJson);

        Debug.Log("Level was Saved");
    }

    private string ParseTilemapLevelData(Tilemap tilemap, string tilemapName)
    {
        // TODO Update functions to use custom ID instead of unity's instance id for saving as JSON
        // Unity changes instances ID each time it starts project or game.
        BoundsInt bounds = tilemap.cellBounds;
        LevelData currentTilemapLevelData = new LevelData();
        currentTilemapLevelData.TilemapName = tilemapName;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (temp != null)
                {
                    currentTilemapLevelData.tiles.Add(temp);
                    currentTilemapLevelData.positions.Add(new Vector3Int(x,y,0));
                }
            }
        }

        return JsonUtility.ToJson(currentTilemapLevelData, true);
    }

    public void LoadMap()
    {
        string json = File.ReadAllText(Application.dataPath + "/" + savesDirectory + "/" + currentLevelName + ".json");
        // TODO Display Loaded Data
        // Load Each Array Element
        // Display Each array Element at Tilemap.
    }
}


public class LevelData
{
    public string TilemapName = "Default";
    public List<TileBase> tiles = new List<TileBase>();
    public List<Vector3Int> positions = new List<Vector3Int>();
}