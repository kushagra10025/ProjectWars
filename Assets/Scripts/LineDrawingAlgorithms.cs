using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum LineDrawingAlgorithm
{
    DDA = 1,
    Bresenham = 2
}

public class LineDrawingAlgorithms : MonoBehaviour
{
    [Tooltip("Bresenham is Experimental")][SerializeField] private LineDrawingAlgorithm _algorithm = LineDrawingAlgorithm.DDA;
    [SerializeField] private Tilemap _buildTilemap;
    [SerializeField] private TileBase _tile;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private bool includeLastCoord = true;

    [SerializeField] private List<Vector3Int> _linePoints;

    private Vector3Int lastEP;
    private Vector3Int currentEP;
    private Vector3Int currentSP;
    private Vector3Int startEP;
    private Vector3Int startSP;

    private void DDA_Algorithm(Vector3Int startPoint, Vector3Int endPoint)
    {
        Vector3 Delta = new Vector3(endPoint.x - startPoint.x, endPoint.y - startPoint.y, endPoint.z - startPoint.z);
        float step = Mathf.Max(Mathf.Abs(Delta.x), Mathf.Abs(Delta.y));

        Delta.x = Delta.x / step;
        Delta.y = Delta.y / step;

        float x = startPoint.x;
        float y = startPoint.y;

        int i = includeLastCoord ? 0 : 1;
        while (i <= step)
        {
            DrawTile(x, y);
            x += Delta.x;
            y += Delta.y;
            i++;
        }
    }

    private void Bresenham_Algorithm(Vector3Int startPoint, Vector3Int endPoint)
    {
        // Currently works for only Slope less than 1
        // TODO Redo to fit any slope
        Vector3Int Delta =
            new Vector3Int(endPoint.x - startPoint.x, endPoint.y - startPoint.y, endPoint.z - startPoint.z);
        int x = startPoint.x;
        int y = startPoint.y;
        int p = 2 * Delta.y - Delta.x;
        while (x < endPoint.x)
        {
            if (p >= 0)
            {
                DrawTile(x, y);
                y = y + 1;
                p = p + 2 * Delta.y - 2 * Delta.x;
            }
            else
            {
                DrawTile(x, y);
                p = p + 2 * Delta.y;
            }
        
            x = x + 1;
        }
    }

    private void DrawTile(float x, float y)
    {
        int RoundX = (int) Mathf.Round(x);
        int RoundY = (int) Mathf.Round(y);
        _linePoints.Add(new Vector3Int(RoundX, RoundY, 0));
        _buildTilemap.SetTile(new Vector3Int(RoundX, RoundY, 0), _tile);
    }

    private void ClearAllTiles()
    {
        _buildTilemap.ClearAllTiles();
    }

    private void Start()
    {
        lastEP = _buildTilemap.WorldToCell(_endPoint.position);
        startSP = _buildTilemap.WorldToCell(_startPoint.position);
        startEP = _buildTilemap.WorldToCell(_endPoint.position);
    }

    private void Update()
    {
        currentSP = _buildTilemap.WorldToCell(_startPoint.position);
        currentEP = _buildTilemap.WorldToCell(_endPoint.position);
        if (!lastEP.Equals(currentEP))
        {
            _linePoints.Clear();
            ClearAllTiles();
            lastEP = currentEP;

            // Update only when EndPoint Changes
            switch (_algorithm)
            {
                case LineDrawingAlgorithm.DDA:
                    DDA_Algorithm(currentSP, currentEP);
                    break;
                case LineDrawingAlgorithm.Bresenham:
                    Bresenham_Algorithm(currentSP, currentEP);
                    break;
            }
        }
    }
}